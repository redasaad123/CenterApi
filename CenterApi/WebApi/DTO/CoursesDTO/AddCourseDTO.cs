using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DTO.CoursesDTO
{
    public class AddCourseDTO
    {

        public string CourseName { get; set; }

        public double Price { get; set; }

        public string Teacher { get; set; }

        public string? TeacherDescription { get; set; }
    }
}
