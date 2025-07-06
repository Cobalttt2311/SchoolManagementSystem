namespace SchoolManagementSystem.Modules.Enrollments.Dtos;

public record EnrollmentRequest(Guid StudentId, Guid ClassId);