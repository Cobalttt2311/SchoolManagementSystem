using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Students.Entities;
using SchoolManagementSystem.Modules.Teachers.Entities;
using SchoolManagementSystem.Modules.Users.Entities;

namespace SchoolManagementSystem.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    public DbSet<Class> Classes { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // =================================================================
        // == PASTIKAN BARIS INI ADA DI PALING ATAS ==
        base.OnModelCreating(modelBuilder);
        // =================================================================

        // Konfigurasi Kunci Komposit untuk tabel Enrollment
        modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.ClassId });
    }
}