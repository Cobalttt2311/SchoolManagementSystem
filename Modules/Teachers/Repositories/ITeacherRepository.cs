using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Teachers.Repositories;

public interface ITeacherRepository
{
    Task<List<Teacher>> GetAllAsync();
    Task<Teacher?> GetByIdAsync(Guid id);
    Task<Teacher> CreateAsync(Teacher teacher);
    Task<Teacher?> UpdateAsync(Teacher teacher);
    Task<bool> DeleteAsync(Guid id);
}