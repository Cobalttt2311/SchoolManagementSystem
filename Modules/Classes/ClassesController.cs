using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Classes.Dtos;
using SchoolManagementSystem.Modules.Classes.Services;

namespace SchoolManagementSystem.Modules.Classes;

[ApiController]
[Route("api/classes")]
[Authorize]
public class ClassesController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassesController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllClasses()
    {
        var classes = await _classService.GetAllClassesAsync();
        return Ok(classes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetClassById(Guid id)
    {
        var classEntity = await _classService.GetClassByIdAsync(id);
        return classEntity is not null ? Ok(classEntity) : NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request)
    {
        var createdClass = await _classService.CreateClassAsync(request);
        return CreatedAtAction(nameof(GetClassById), new { id = createdClass.Id }, createdClass);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> UpdateClass(Guid id, [FromBody] UpdateClassRequest request)
    {
        try
        {
            var updatedClass = await _classService.UpdateClassAsync(id, request);
            return updatedClass is not null ? Ok(updatedClass) : NotFound();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpPut("{id:guid}/assign-teacher")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignTeacher(Guid id, [FromBody] AssignTeacherRequest request)
    {
        try
        {
            var updatedClass = await _classService.AssignTeacherAsync(id, request.TeacherId);
            return updatedClass is not null ? Ok(updatedClass) : NotFound();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClass(Guid id)
    {
        var result = await _classService.DeleteClassAsync(id);
        return result ? NoContent() : NotFound();
    }
}