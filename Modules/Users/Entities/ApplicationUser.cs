using Microsoft.AspNetCore.Identity;

namespace SchoolManagementSystem.Modules.Users.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? StudentId { get; set; }
        public Guid? TeacherId { get; set; }
    }
}