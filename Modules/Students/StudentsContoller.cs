using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Students.Dtos;
using SchoolManagementSystem.Modules.Students.Services;

namespace SchoolManagementSystem.Modules.Students;

[ApiController]
[Route("api/students")]
[Authorize] // Semua endpoint di sini minimal harus login
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Teacher")] // Hanya Admin & Teacher yang bisa lihat semua siswa
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }


    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")] // Hanya Admin & Teacher yang bisa lihat detail siswa
    public async Task<IActionResult> GetStudentById(Guid id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        return student is not null ? Ok(student) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Hanya Admin yang bisa membuat siswa
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var createdStudent = await _studentService.CreateStudentAsync(request);
        return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")] // Hanya Admin yang bisa mengedit siswa
    public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] UpdateStudentRequest request)
    {
        var updatedStudent = await _studentService.UpdateStudentAsync(id, request);
        return updatedStudent is not null ? Ok(updatedStudent) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")] // Hanya Admin yang bisa menghapus siswa
    public async Task<IActionResult> DeleteStudent(Guid id)
    {
        var result = await _studentService.DeleteStudentAsync(id);
        return result ? NoContent() : NotFound();
    }
}