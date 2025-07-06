using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Modules.Classes.Dtos;
using SchoolManagementSystem.Modules.Classes.Mappers;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Enrollments.Repositories;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using SchoolManagementSystem.Modules.Users.Entities;
using System.Security.Claims;

namespace SchoolManagementSystem.Modules.Classes.Services;

public class ClassService : IClassService
{
    private readonly IClassRepository _classRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClassService(
        IClassRepository classRepository,
        ITeacherRepository teacherRepository,
        IEnrollmentRepository enrollmentRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        _classRepository = classRepository;
        _teacherRepository = teacherRepository;
        _enrollmentRepository = enrollmentRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<List<ClassResponse>> GetAllClassesAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        var user = httpContext.User;

        // Hanya Admin yang bisa melihat semua kelas tanpa filter
        if (user.IsInRole("Admin"))
        {
            var allClasses = await _classRepository.GetAllAsync();
            return allClasses.Select(c => c.ToClassResponse()).ToList();
        }
        
        // Teacher hanya melihat kelas miliknya
        if (user.IsInRole("Teacher"))
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var appUser = await _userManager.FindByIdAsync(userId!);
            var teacherId = appUser?.TeacherId;

            if (teacherId.HasValue)
            {
                var teacherClasses = await _classRepository.GetAllByTeacherIdAsync(teacherId.Value);
                return teacherClasses.Select(c => c.ToClassResponse()).ToList();
            }
        }
        
        // Student tidak bisa melihat daftar semua kelas lewat endpoint ini.
        return new List<ClassResponse>();
    }

    public async Task<ClassResponse?> GetClassByIdAsync(Guid id)
    {
        var classEntity = await _classRepository.GetByIdAsync(id);
        if (classEntity is null) return null;

        var httpContext = _httpContextAccessor.HttpContext!;
        var user = httpContext.User;

        if (user.IsInRole("Admin")) return classEntity.ToClassResponse();

        if (user.IsInRole("Teacher"))
        {
            var teacherUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacherUser = await _userManager.FindByIdAsync(teacherUserId!);
            if (teacherUser?.TeacherId == classEntity.TeacherId)
            {
                return classEntity.ToClassResponse();
            }
        }

        if (user.IsInRole("Student"))
        {
            var studentUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            var studentUser = await _userManager.FindByIdAsync(studentUserId!);
            if (studentUser?.StudentId.HasValue ?? false)
            {
                bool isEnrolled = await _enrollmentRepository.IsStudentEnrolledAsync(studentUser.StudentId.Value, id);
                if (isEnrolled)
                {
                    return classEntity.ToClassResponse();
                }
            }
        }
        
        return null;
    }

    public async Task<ClassResponse> CreateClassAsync(CreateClassRequest request)
    {
        var classEntity = request.ToClassEntity();
        var createdClass = await _classRepository.CreateAsync(classEntity);
        var result = await _classRepository.GetByIdAsync(createdClass.Id);
        return result!.ToClassResponse();
    }

    public async Task<ClassResponse?> UpdateClassAsync(Guid id, UpdateClassRequest request)
    {
        var classToUpdate = await _classRepository.GetByIdAsync(id);
        if (classToUpdate is null) return null;
        
        await CheckOwnership(classToUpdate.TeacherId);
        
        classToUpdate.ClassName = request.ClassName;
        classToUpdate.Schedule = request.Schedule;
        
        var updatedClass = await _classRepository.UpdateAsync(classToUpdate);
        return updatedClass?.ToClassResponse();
    }

    public async Task<ClassResponse?> AssignTeacherAsync(Guid classId, Guid? teacherId)
    {
        var classToUpdate = await _classRepository.GetByIdAsync(classId);
        if (classToUpdate is null) return null;

        await CheckOwnership(classToUpdate.TeacherId);

        if (teacherId.HasValue && await _teacherRepository.GetByIdAsync(teacherId.Value) is null)
        {
            throw new KeyNotFoundException("Teacher with the specified ID not found.");
        }
        
        classToUpdate.TeacherId = teacherId;
        var updatedClass = await _classRepository.UpdateAsync(classToUpdate);
        return updatedClass?.ToClassResponse();
    }

    public async Task<bool> DeleteClassAsync(Guid id)
    {
        return await _classRepository.DeleteAsync(id);
    }

    private async Task CheckOwnership(Guid? classTeacherId)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        if (user.IsInRole("Admin")) return;

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var appUser = await _userManager.FindByIdAsync(userId!);
        
        if (appUser?.TeacherId != classTeacherId)
        {
            throw new UnauthorizedAccessException("You are not authorized to modify this class.");
        }
    }
}