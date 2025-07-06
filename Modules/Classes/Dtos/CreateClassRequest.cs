namespace SchoolManagementSystem.Modules.Classes.Dtos;

public record CreateClassRequest(string ClassName, string Schedule, Guid? TeacherId);
