namespace SchoolManagementSystem.Modules.Students.Dtos;

public record StudentResponse(Guid Id, string Name, string Email, DateOnly DateOfBirth);