using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Materails
    {
        [Key]
        public string materailId { get; set; }
        
        public string materailName { get; set; }

        public string? materailVideo {  get; set; }

        public string? materailPdf { get; set; }


        [ForeignKey("CourseId")]
        public Courses Course { get; set; }

        public string CourseId { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
