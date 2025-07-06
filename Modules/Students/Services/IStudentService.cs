using SchoolManagementSystem.Modules.Students.Dtos;

namespace SchoolManagementSystem.Modules.Students.Services;

public interface IStudentService
{
    Task<List<StudentResponse>> GetAllStudentsAsync();
    Task<StudentResponse?> GetStudentByIdAsync(Guid id);
    Task<StudentResponse> CreateStudentAsync(CreateStudentRequest request);
    Task<StudentResponse?> UpdateStudentAsync(Guid id, UpdateStudentRequest request);
    Task<bool> DeleteStudentAsync(Guid id);
}