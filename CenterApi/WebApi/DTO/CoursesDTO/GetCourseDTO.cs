using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DTO.CoursesDTO
{
    public class GetCourseDto
    {
        public string CourseId { get; set; }

        public string CourseName { get; set; }


        public string Teacher { get; set; }
        public double Price { get; set; }

        public string TeacherDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
