using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Students.Repositories;

public interface IStudentRepository
{
    Task<Student> CreateAsync(Student student);
    Task<List<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student?> UpdateAsync(Student student);
    Task<bool> DeleteAsync(Guid id);

    Task<Student?> GetByEmailAsync(string email);

    Task<List<Student>> GetAllByTeacherIdAsync(Guid teacherId);
}