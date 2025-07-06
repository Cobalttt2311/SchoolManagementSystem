using SchoolManagementSystem.Modules.Teachers.Dtos;
using SchoolManagementSystem.Modules.Teachers.Entities;

namespace SchoolManagementSystem.Modules.Teachers.Mappers;

public static class TeacherMapper
{
    // Mengubah Entity Teacher -> DTO TeacherResponse
    public static TeacherResponse ToTeacherResponse(this Teacher teacher)
    {
        return new TeacherResponse(
            teacher.Id,
            teacher.Name,
            teacher.SubjectSpecialization
        );
    }

    // Mengubah DTO CreateTeacherRequest -> Entity Teacher
    public static Teacher ToTeacherEntity(this CreateTeacherRequest request)
    {
        return new Teacher
        {
            Name = request.Name,
            SubjectSpecialization = request.SubjectSpecialization
        };
    }
}