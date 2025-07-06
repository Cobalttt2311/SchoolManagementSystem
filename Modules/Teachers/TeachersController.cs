using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Teachers.Dtos;
using SchoolManagementSystem.Modules.Teachers.Services;

namespace SchoolManagementSystem.Modules.Teachers;

[ApiController]
[Route("api/teachers")]
[Authorize(Roles = "Admin")] // Seluruh controller ini hanya bisa diakses Admin
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTeachers()
    {
        var teachers = await _teacherService.GetAllTeachersAsync();
        return Ok(teachers);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTeacherById(Guid id)
    {
        var teacher = await _teacherService.GetTeacherByIdAsync(id);
        return teacher is not null ? Ok(teacher) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherRequest request)
    {
        var createdTeacher = await _teacherService.CreateTeacherAsync(request);
        return CreatedAtAction(nameof(GetTeacherById), new { id = createdTeacher.Id }, createdTeacher);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTeacher(Guid id, [FromBody] UpdateTeacherRequest request)
    {
        var updatedTeacher = await _teacherService.UpdateTeacherAsync(id, request);
        return updatedTeacher is not null ? Ok(updatedTeacher) : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTeacher(Guid id)
    {
        var result = await _teacherService.DeleteTeacherAsync(id);
        return result ? NoContent() : NotFound();
    }
}