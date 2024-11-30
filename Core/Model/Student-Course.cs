using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Student_Course
    {
        public string Id { get; set; }
        [ForeignKey("StdId")]
        public AppUser student { get; set; }

        public string StdId { get; set; }

        [ForeignKey("courseId")]
        public Courses course { get; set; }

        public string courseId { get; set; }





    }
}
