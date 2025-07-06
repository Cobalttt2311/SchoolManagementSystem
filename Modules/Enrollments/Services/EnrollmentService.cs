using Microsoft.AspNetCore.Identity;
using SchoolManagementSystem.Modules.Classes.Repositories;
using SchoolManagementSystem.Modules.Enrollments.Dtos;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Enrollments.Mappers;
using SchoolManagementSystem.Modules.Enrollments.Repositories;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Users.Entities;
using System.Security.Claims;

namespace SchoolManagementSystem.Modules.Enrollments.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly IClassRepository _classRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<EnrollmentService> _logger;

    public EnrollmentService(
        IEnrollmentRepository enrollmentRepository,
        IStudentRepository studentRepository,
        IClassRepository classRepository,
        IHttpContextAccessor httpContextAccessor,
        UserManager<ApplicationUser> userManager,
        ILogger<EnrollmentService> logger)
    {
        _enrollmentRepository = enrollmentRepository;
        _studentRepository = studentRepository;
        _classRepository = classRepository;
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<EnrollmentResponse>> GetAllEnrollmentsAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext!;
        if (httpContext.User.IsInRole("Admin"))
        {
            var allEnrollments = await _enrollmentRepository.GetAllAsync();
            return allEnrollments.Select(e => e.ToEnrollmentResponse()).ToList();
        }
        
        return new List<EnrollmentResponse>();
    }

    public async Task<List<EnrollmentResponse>> GetMyEnrollmentsAsync()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation("Mencoba mengambil data untuk User ID dari token: {UserId}", userId);
        if (userId is null) return new List<EnrollmentResponse>();

        var user = await _userManager.FindByIdAsync(userId);
        var studentId = user?.StudentId;
        _logger.LogInformation("Profil user ditemukan, Student ID yang tertaut: {StudentId}", studentId);

        if (!studentId.HasValue)
        {
            _logger.LogWarning("Student ID tidak tertaut ke akun ini. Mengembalikan list kosong.");
            return new List<EnrollmentResponse>();
        }

        var enrollments = await _enrollmentRepository.GetByStudentIdAsync(studentId.Value);
        _logger.LogInformation("Ditemukan {EnrollmentCount} pendaftaran untuk Student ID ini.", enrollments.Count);
        
        return enrollments.Select(e => e.ToEnrollmentResponse()).ToList();
    }

    public async Task<EnrollmentResponse> EnrollStudentAsync(EnrollmentRequest request)
    {
        var studentExists = await _studentRepository.GetByIdAsync(request.StudentId);
        if (studentExists is null) throw new KeyNotFoundException("Student not found.");

        var classExists = await _classRepository.GetByIdAsync(request.ClassId);
        if (classExists is null) throw new KeyNotFoundException("Class not found.");

        var existingEnrollment = await _enrollmentRepository.GetByIdAsync(request.StudentId, request.ClassId);
        if (existingEnrollment is not null)
        {
            throw new InvalidOperationException("Student is already enrolled in this class.");
        }

        var newEnrollment = new Enrollment
        {
            StudentId = request.StudentId,
            ClassId = request.ClassId,
            EnrollmentDate = DateTime.UtcNow
        };

        await _enrollmentRepository.CreateAsync(newEnrollment);
        
        var createdEnrollmentWithIncludes = await _enrollmentRepository.GetByIdAsync(newEnrollment.StudentId, newEnrollment.ClassId);
        return createdEnrollmentWithIncludes!.ToEnrollmentResponse();
    }

    public async Task<bool> UnenrollStudentAsync(Guid studentId, Guid classId)
    {
        return await _enrollmentRepository.DeleteAsync(studentId, classId);
    }
}