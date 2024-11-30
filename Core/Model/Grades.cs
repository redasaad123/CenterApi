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
    public class Grades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string gradeId { get; set; }
        [ForeignKey("StudentId")]
        [JsonIgnore]
        public AppUser Student { get; set; }

        public string StudentId { get; set; }

        [ForeignKey("CourseId")]
        public Courses Courses { get; set; }

        public string CourseId { get; set; }


        public double grade { get; set; }
    }
}
