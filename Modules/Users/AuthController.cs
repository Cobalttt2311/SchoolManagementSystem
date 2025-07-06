using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SchoolManagementSystem.Modules.Students.Repositories;
using SchoolManagementSystem.Modules.Teachers.Repositories;
using SchoolManagementSystem.Modules.Users.Dtos;
using SchoolManagementSystem.Modules.Users.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SchoolManagementSystem.Modules.Users;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IStudentRepository _studentRepository;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration,
        ITeacherRepository teacherRepository,
        IStudentRepository studentRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _teacherRepository = teacherRepository;
        _studentRepository = studentRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
        {
            return BadRequest(new { message = "User login with this email already exists." });
        }

        if (request.Role == "Admin")
        {
            return BadRequest(new { message = "Admin registration is not allowed." });
        }
        
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new { message = "User creation failed.", errors = result.Errors });
        }

        if (request.Role == "Teacher")
        {
            if (!request.TeacherId.HasValue) return BadRequest(new { message = "TeacherId is required." });
            var teacher = await _teacherRepository.GetByIdAsync(request.TeacherId.Value);
            if (teacher is null) return NotFound(new { message = "Teacher profile not found." });
            if (await _userManager.Users.AnyAsync(u => u.TeacherId == request.TeacherId.Value))
            {
                return Conflict(new { message = "This teacher profile is already linked." });
            }
            user.TeacherId = teacher.Id;
        }
        else if (request.Role == "Student")
        {
            var student = await _studentRepository.GetByEmailAsync(request.Email);
            if (student is null)
            {
                // Hapus user login yang sudah terlanjur dibuat jika profil student tidak ada
                await _userManager.DeleteAsync(user);
                return NotFound(new { message = "Student profile with this email not found." });
            }
            if (await _userManager.Users.AnyAsync(u => u.StudentId == student.Id))
            {
                return Conflict(new { message = "This student profile is already linked." });
            }
            user.StudentId = student.Id;
        }

        // Simpan perubahan (TeacherId/StudentId) ke database
        await _userManager.UpdateAsync(user);

        // Tambahkan role ke user
        await _userManager.AddToRoleAsync(user, request.Role);

        return Ok(new { message = $"User for {request.Role} created successfully!" });
    }
    
    // ... metode Login dan GenerateJwtToken tetap sama ...
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new { message = "Invalid credentials." });
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = GenerateJwtToken(authClaims);

        return Ok(new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo
        });
    }

    private JwtSecurityToken GenerateJwtToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            expires: DateTime.Now.AddHours(3),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
}