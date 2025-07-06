using SchoolManagementSystem.Modules.Classes.Entities;

namespace SchoolManagementSystem.Modules.Classes.Repositories;

public interface IClassRepository
{
    // Kita pakai Include untuk mengambil data Teacher sekaligus
    Task<List<Class>> GetAllAsync();
    Task<Class?> GetByIdAsync(Guid id);
    Task<Class> CreateAsync(Class newClass);
    Task<Class?> UpdateAsync(Class updatedClass);
    Task<bool> DeleteAsync(Guid id);
    // Tambahkan metode ini
    Task<List<Class>> GetAllByTeacherIdAsync(Guid teacherId);
}