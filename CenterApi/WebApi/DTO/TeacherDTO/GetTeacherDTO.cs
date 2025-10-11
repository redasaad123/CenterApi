namespace WebApi.DTO.TeacherDTO
{
    public class GetTeacherDTO
    {
        public string teacherId { get; set; }

        public string teacherName { get; set; }

        public IEnumerable< string> courseName { get; set; }
    }
}
