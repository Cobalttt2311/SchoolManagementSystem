using SchoolManagementSystem.Modules.Teachers.Dtos;
using SchoolManagementSystem.Modules.Teachers.Mappers;
using SchoolManagementSystem.Modules.Teachers.Repositories;

namespace SchoolManagementSystem.Modules.Teachers.Services;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _teacherRepository;

    public TeacherService(ITeacherRepository teacherRepository)
    {
        _teacherRepository = teacherRepository;
    }

    public async Task<List<TeacherResponse>> GetAllTeachersAsync()
    {
        var teachers = await _teacherRepository.GetAllAsync();
        return teachers.Select(t => t.ToTeacherResponse()).ToList();
    }

    public async Task<TeacherResponse?> GetTeacherByIdAsync(Guid id)
    {
        var teacher = await _teacherRepository.GetByIdAsync(id);
        return teacher?.ToTeacherResponse();
    }

    public async Task<TeacherResponse> CreateTeacherAsync(CreateTeacherRequest request)
    {
        var teacherEntity = request.ToTeacherEntity();
        var createdTeacher = await _teacherRepository.CreateAsync(teacherEntity);
        return createdTeacher.ToTeacherResponse();
    }

    public async Task<TeacherResponse?> UpdateTeacherAsync(Guid id, UpdateTeacherRequest request)
    {
        var existingTeacher = await _teacherRepository.GetByIdAsync(id);
        if (existingTeacher is null)
        {
            return null;
        }

        existingTeacher.Name = request.Name;
        existingTeacher.SubjectSpecialization = request.SubjectSpecialization;

        var updatedTeacher = await _teacherRepository.UpdateAsync(existingTeacher);
        
        return updatedTeacher?.ToTeacherResponse();
    }

    public async Task<bool> DeleteTeacherAsync(Guid id)
    {
        return await _teacherRepository.DeleteAsync(id);
    }
}