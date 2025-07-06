namespace SchoolManagementSystem.Modules.Students.Dtos;

public record CreateStudentRequest(string Name, string Email, DateOnly DateOfBirth);