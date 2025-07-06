// Tambahkan using untuk Teacher dan Enrollment
using SchoolManagementSystem.Modules.Enrollments.Entities;
using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Classes.Entities
{
    public class Class
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Schedule { get; set; } = string.Empty;

        public Guid? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}