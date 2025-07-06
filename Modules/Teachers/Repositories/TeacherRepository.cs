using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Teachers.Repositories;

public class TeacherRepository : ITeacherRepository
{
    private readonly AppDbContext _context;

    public TeacherRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Teacher> CreateAsync(Teacher teacher)
    {
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();
        return teacher;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher is null) return false;

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Teacher>> GetAllAsync()
    {
        return await _context.Teachers.ToListAsync();
    }

    public async Task<Teacher?> GetByIdAsync(Guid id)
    {
        return await _context.Teachers.FindAsync(id);
    }

    public async Task<Teacher?> UpdateAsync(Teacher teacher)
    {
        var existingTeacher = await _context.Teachers.FindAsync(teacher.Id);
        if (existingTeacher is null) return null;

        existingTeacher.Name = teacher.Name;
        existingTeacher.SubjectSpecialization = teacher.SubjectSpecialization;
        
        await _context.SaveChangesAsync();
        return existingTeacher;
    }
}