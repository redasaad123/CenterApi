using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class TeacherCourses
    {
        [Key]
        public string TCoursesId {  get; set; }

        [ForeignKey("courseId")]
        public Courses Courses { get; set; }
        public string courseId { get; set; }


        [ForeignKey("TeacherId")]
        public AppUser AppUser { get; set; }
        public string TeacherId { get; set; }

    }
}
