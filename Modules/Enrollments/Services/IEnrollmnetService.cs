using SchoolManagementSystem.Modules.Enrollments.Dtos;

namespace SchoolManagementSystem.Modules.Enrollments.Services;

public interface IEnrollmentService
{
    Task<List<EnrollmentResponse>> GetAllEnrollmentsAsync();
    Task<EnrollmentResponse> EnrollStudentAsync(EnrollmentRequest request);
    Task<bool> UnenrollStudentAsync(Guid studentId, Guid classId);
    Task<List<EnrollmentResponse>> GetMyEnrollmentsAsync();
}