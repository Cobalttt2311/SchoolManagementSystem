// Tambahkan using untuk Class
using SchoolManagementSystem.Modules.Classes.Entities;

// Ubah namespace menjadi jamak
namespace SchoolManagementSystem.Modules.Teachers.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubjectSpecialization { get; set; } = string.Empty;

        public ICollection<Class> Classes { get; set; } = new List<Class>();
    }
}