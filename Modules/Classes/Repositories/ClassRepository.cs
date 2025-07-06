using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Modules.Classes.Entities;

namespace SchoolManagementSystem.Modules.Classes.Repositories;

public class ClassRepository : IClassRepository
{
    private readonly AppDbContext _context;

    public ClassRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Class> CreateAsync(Class newClass)
    {
        await _context.Classes.AddAsync(newClass);
        await _context.SaveChangesAsync();
        return newClass;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var classToDelete = await _context.Classes.FindAsync(id);
        if (classToDelete is null) return false;

        _context.Classes.Remove(classToDelete);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Class>> GetAllAsync()
    {
        // .Include() mengambil data relasi (Teacher) dalam satu query
        return await _context.Classes.Include(c => c.Teacher).ToListAsync();
    }

    public async Task<Class?> GetByIdAsync(Guid id)
    {
        return await _context.Classes.Include(c => c.Teacher).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Class?> UpdateAsync(Class updatedClass)
    {
        _context.Classes.Update(updatedClass);
        await _context.SaveChangesAsync();
        return updatedClass;
    }
    
    // Tambahkan implementasi ini
    public async Task<List<Class>> GetAllByTeacherIdAsync(Guid teacherId)
    {
        return await _context.Classes
            .Where(c => c.TeacherId == teacherId)
            .Include(c => c.Teacher)
            .ToListAsync();
    }
}