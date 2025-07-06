using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Modules.Students.Dtos;
using SchoolManagementSystem.Modules.Students.Mappers;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Users.Entities;
using System.Security.Claims;

namespace SchoolManagementSystem.Modules.Students.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;

    public StudentService(
        IStudentRepository studentRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager)
    {
        _studentRepository = studentRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public async Task<List<StudentResponse>> GetAllStudentsAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (httpContext.User.IsInRole("Admin"))
        {
            var allStudents = await _studentRepository.GetAllAsync();
            return allStudents.Select(s => s.ToStudentResponse()).ToList();
        }
        if (httpContext.User.IsInRole("Teacher"))
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);
            if (user?.TeacherId.HasValue ?? false)
            {
                var studentsInClass = await _studentRepository.GetAllByTeacherIdAsync(user.TeacherId.Value);
                return studentsInClass.Select(s => s.ToStudentResponse()).ToList();
            }
        }
        return new List<StudentResponse>();
    }

    public async Task<StudentResponse?> GetStudentByIdAsync(Guid id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        return student?.ToStudentResponse();
    }

    public async Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request)
    {
        var studentEntity = request.ToStudentEntity();
        var createdStudent = await _studentRepository.CreateAsync(studentEntity);
        return createdStudent.ToStudentResponse();
    }

    public async Task<StudentResponse?> UpdateStudentAsync(Guid id, UpdateStudentRequest request)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(id);
        if (existingStudent is null)
        {
            return null;
        }
        existingStudent.Name = request.Name;
        existingStudent.Email = request.Email;
        existingStudent.DateOfBirth = request.DateOfBirth;
        var updatedStudent = await _studentRepository.UpdateAsync(existingStudent);
        return updatedStudent?.ToStudentResponse();
    }

    public async Task<bool> DeleteStudentAsync(Guid id)
    {
        return await _studentRepository.DeleteAsync(id);
    }
}