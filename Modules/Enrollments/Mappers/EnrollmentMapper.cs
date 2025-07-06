using SchoolManagementSystem.Modules.Enrollments.Dtos;
using SchoolManagementSystem.Modules.Enrollments.Entities;

namespace SchoolManagementSystem.Modules.Enrollments.Mappers;

public static class EnrollmentMapper
{
    // Mengubah Entity Enrollment -> DTO EnrollmentResponse
    public static EnrollmentResponse ToEnrollmentResponse(this Enrollment enrollment)
    {
        var studentInfo = new EnrolledStudentInfo(
            enrollment.Student.Id,
            enrollment.Student.Name
        );

        var classInfo = new EnrolledClassInfo(
            enrollment.Class.Id,
            enrollment.Class.ClassName
        );

        return new EnrollmentResponse(
            studentInfo,
            classInfo,
            enrollment.EnrollmentDate
        );
    }
}