using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Data;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Students.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Student> CreateAsync(Student student)
    {
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();
        return student;
    }

    public async Task<List<Student>> GetAllAsync()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student?> UpdateAsync(Student student)
    {
        var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Id == student.Id);

        if (existingStudent is null)
        {
            return null;
        }

        // Update properti
        existingStudent.Name = student.Name;
        existingStudent.Email = student.Email;
        existingStudent.DateOfBirth = student.DateOfBirth;

        await _context.SaveChangesAsync();
        return existingStudent;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var studentToDelete = await _context.Students.FirstOrDefaultAsync(s => s.Id == id);

        if (studentToDelete is null)
        {
            return false;
        }

        _context.Students.Remove(studentToDelete);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<Student?> GetByEmailAsync(string email)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.Email == email);
    }

    // Tambahkan implementasi ini
    public async Task<List<Student>> GetAllByTeacherIdAsync(Guid teacherId)
    {
        return await _context.Students
            .Where(s => s.Enrollments.Any(e => e.Class.TeacherId == teacherId))
            .Distinct()
            .ToListAsync();
    }
}