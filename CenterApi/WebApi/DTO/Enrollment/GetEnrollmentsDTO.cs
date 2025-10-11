namespace WebApi.DTO.Enrollment
{
    public class GetEnrollmentsDTO : GetEnrollmentInCourseDTO
    {
        public string CourseName { get; set; }

        public string TeacherName { get; set; }

    }
}
