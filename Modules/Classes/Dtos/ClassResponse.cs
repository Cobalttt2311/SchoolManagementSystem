namespace SchoolManagementSystem.Modules.Classes.Dtos;
public record TeacherInfo(Guid? Id, string? Name);

public record ClassResponse(
    Guid Id,
    string ClassName,
    string Schedule,
    TeacherInfo? Teacher
);

