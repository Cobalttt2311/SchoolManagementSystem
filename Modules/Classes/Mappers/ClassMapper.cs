using SchoolManagementSystem.Modules.Classes.Dtos;
using SchoolManagementSystem.Modules.Classes.Entities;

namespace SchoolManagementSystem.Modules.Classes.Mappers;

public static class ClassMapper
{
    // Mengubah Entity Class -> DTO ClassResponse
    public static ClassResponse ToClassResponse(this Class classEntity)
    {
        // Membuat DTO untuk info guru jika ada
        var teacherInfo = classEntity.Teacher is null
            ? null
            : new TeacherInfo(classEntity.Teacher.Id, classEntity.Teacher.Name);

        return new ClassResponse(
            classEntity.Id,
            classEntity.ClassName,
            classEntity.Schedule,
            teacherInfo
        );
    }

    // Mengubah DTO CreateClassRequest -> Entity Class
    public static Class ToClassEntity(this CreateClassRequest request)
    {
        return new Class
        {
            ClassName = request.ClassName,
            Schedule = request.Schedule,
            TeacherId = request.TeacherId
        };
    }
}