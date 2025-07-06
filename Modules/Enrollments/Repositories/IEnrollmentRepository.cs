using SchoolManagementSystem.Modules.Enrollments.Entities;

namespace SchoolManagementSystem.Modules.Enrollments.Repositories;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetAllAsync();
    Task<Enrollment?> GetByIdAsync(Guid studentId, Guid classId);
    Task<Enrollment> CreateAsync(Enrollment enrollment);
    Task<bool> DeleteAsync(Guid studentId, Guid classId);
    Task<List<Enrollment>> GetByStudentIdAsync(Guid studentId);
    Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid classId);
}