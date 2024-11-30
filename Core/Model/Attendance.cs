using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Attendance
    {
        [Key]
        public string AttendId { get; set; }

        public DateTime AttendDate { get; set; }

        [ForeignKey("CourseId")]
        public Courses Course { get; set; }

        public string CourseId { get;  set; }

        [ForeignKey("StudentId")]
        public AppUser Student { get; set; }

        public string StudentId { get;  set; }

    }
}
