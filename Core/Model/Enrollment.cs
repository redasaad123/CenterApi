using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Enrollment
    {
        [Key]
        public string EnrollmentId { get; set; }

        public DateTime enrolledAt { get; set; }

        [ForeignKey("courseId")]
        [JsonIgnore]
        public Courses course { get; set; }

        public string courseId { get; set; }

        [ForeignKey("studentId")]
        [JsonIgnore]
        public AppUser student { get; set; }

        public string studentId { get; set; }
    }
}
