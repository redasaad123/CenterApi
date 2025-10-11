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
    public class Courses
    {
        [Key]
        public string CourseId { get; set; }

        public string CourseName { get;set; }

        public double Price { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("TeacherId")]
        [JsonIgnore]
        public AppUser Teacher { get; set; }

        public string TeacherId { get;set; }

        public string? TeacherDescription { get; set; }


    }
}
