namespace SchoolManagementSystem.Modules.Enrollments.Dtos;

// DTO sederhana untuk menampilkan info di dalam response
public record EnrolledStudentInfo(Guid Id, string Name);
public record EnrolledClassInfo(Guid Id, string ClassName);

public record EnrollmentResponse(
    EnrolledStudentInfo Student,
    EnrolledClassInfo Class,
    DateTime EnrollmentDate
);