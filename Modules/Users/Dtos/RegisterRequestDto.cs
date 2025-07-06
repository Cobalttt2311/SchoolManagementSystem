using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Modules.Users.Dtos;

public class RegisterRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = string.Empty;

    // Properti opsional, hanya diisi jika mendaftarkan Teacher
    public Guid? TeacherId { get; set; }
}