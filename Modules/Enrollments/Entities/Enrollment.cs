// Tambahkan using untuk Student dan Class
using SchoolManagementSystem.Modules.Classes.Entities;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Enrollments.Entities
{
    public class Enrollment
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!; // null! untuk atasi warning, EF akan mengisinya

        public Guid ClassId { get; set; }
        public Class Class { get; set; } = null!; // null! untuk atasi warning, EF akan mengisinya

        public DateTime EnrollmentDate { get; set; }
    }
}