namespace SchoolManagementSystem.Modules.Users.Dtos;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}