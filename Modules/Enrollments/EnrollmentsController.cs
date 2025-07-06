using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Modules.Enrollments.Dtos;
using SchoolManagementSystem.Modules.Enrollments.Services;

namespace SchoolManagementSystem.Modules.Enrollments;

[ApiController]
[Route("api/enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllEnrollments()
    {
        var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
        return Ok(enrollments);
    }
    
    [HttpGet("me")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyEnrollments()
    {
        var enrollments = await _enrollmentService.GetMyEnrollmentsAsync();
        return Ok(enrollments);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> EnrollStudent([FromBody] EnrollmentRequest request)
    {
        try
        {
            var result = await _enrollmentService.EnrollStudentAsync(request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,Teacher")]
    public async Task<IActionResult> UnenrollStudent([FromBody] EnrollmentRequest request)
    {
        var result = await _enrollmentService.UnenrollStudentAsync(request.StudentId, request.ClassId);
        return result ? NoContent() : NotFound(new { message = "Enrollment not found."});
    }
}