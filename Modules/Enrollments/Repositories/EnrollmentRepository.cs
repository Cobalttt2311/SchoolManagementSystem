using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Modules.Enrollments.Entities;

namespace SchoolManagementSystem.Modules.Enrollments.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly AppDbContext _context;

    public EnrollmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Enrollment> CreateAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();
        return enrollment;
    }

    public async Task<bool> DeleteAsync(Guid studentId, Guid classId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);

        if (enrollment is null) return false;

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        // Wajib .Include() untuk mengambil data relasi Student dan Class
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Class)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(Guid studentId, Guid classId)
    {
        return await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.ClassId == classId);
    }

    public async Task<List<Enrollment>> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.Enrollments
            .Where(e => e.StudentId == studentId)
            .Include(e => e.Class)
            .Include(e => e.Student)
            .ToListAsync();
    }

    public async Task<bool> IsStudentEnrolledAsync(Guid studentId, Guid classId)
    {
        return await _context.Enrollments.AnyAsync(e => e.StudentId == studentId && e.ClassId == classId);
    }
}