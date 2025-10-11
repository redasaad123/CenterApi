using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class SolveTask
    {
        [Key]
        public string solveTaskId {  get; set; }

        public DateTime SolvedAt { get; set; }

        public string solveURl {  get; set; }

        [ForeignKey("taskId")]
        public Tasks task { get; set; }

        public string taskId { get; set; }

        [ForeignKey("studentId")]
        public AppUser student { get; set; }

        public string studentId { get; set; }




    }
}
