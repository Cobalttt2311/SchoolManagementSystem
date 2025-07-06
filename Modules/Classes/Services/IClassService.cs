using SchoolManagementSystem.Modules.Classes.Dtos;

namespace SchoolManagementSystem.Modules.Classes.Services;

public interface IClassService
{
    Task<List<ClassResponse>> GetAllClassesAsync();
    Task<ClassResponse?> GetClassByIdAsync(Guid id);
    Task<ClassResponse> CreateClassAsync(CreateClassRequest request);
    Task<ClassResponse?> UpdateClassAsync(Guid id, UpdateClassRequest request);
    Task<ClassResponse?> AssignTeacherAsync(Guid classId, Guid? teacherId);
    Task<bool> DeleteClassAsync(Guid id);
}