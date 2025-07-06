namespace SchoolManagementSystem.Modules.Students.Dtos;

public record UpdateStudentRequest(string Name, string Email, DateOnly DateOfBirth);