using SchoolManagementSystem.Modules.Students.Dtos;
using SchoolManagementSystem.Modules.Students.Entities;

namespace SchoolManagementSystem.Modules.Students.Mappers;

public static class StudentMapper
{
    // Mengubah Entity Student -> DTO StudentResponse
    public static StudentResponse ToStudentResponse(this Student student)
    {
        return new StudentResponse(
            student.Id,
            student.Name,
            student.Email,
            student.DateOfBirth
        );
    }

    // Mengubah DTO CreateStudentRequest -> Entity Student
    public static Student ToStudentEntity(this CreateStudentRequest request)
    {
        return new Student
        {
            Name = request.Name,
            Email = request.Email,
            DateOfBirth = request.DateOfBirth
        };
    }
}