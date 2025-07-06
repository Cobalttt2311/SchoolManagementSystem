using SchoolManagementSystem.Modules.Teachers.Dtos;

namespace SchoolManagementSystem.Modules.Teachers.Services;

public interface ITeacherService
{
    Task<List<TeacherResponse>> GetAllTeachersAsync();
    Task<TeacherResponse?> GetTeacherByIdAsync(Guid id);
    Task<TeacherResponse> CreateTeacherAsync(CreateTeacherRequest request);
    Task<TeacherResponse?> UpdateTeacherAsync(Guid id, UpdateTeacherRequest request);
    Task<bool> DeleteTeacherAsync(Guid id);
}