using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class Tasks
    {
        [Key]
        public string TaskId { get; set; }
        public string TaskName { get; set; }



        public string Time {  get; set; }


        public string UrlTask {  get; set; }

        [ForeignKey("CourseId")]
        public Courses Course { get; set; }

        public string CourseId { get; set; }
        public DateTime DateTask { get; set; }
    }
}
