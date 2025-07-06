// Tambahkan using untuk Enrollment
using SchoolManagementSystem.Modules.Enrollments.Entities;

// Ubah namespace menjadi jamak
namespace SchoolManagementSystem.Modules.Students.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // Inisialisasi untuk atasi warning
        public string Email { get; set; } = string.Empty; // Inisialisasi untuk atasi warning
        public DateOnly DateOfBirth { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}