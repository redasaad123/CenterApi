using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class feedBack
    {
        [Key]
        public string MessageId { get; set; }

        public string Message { get; set; }

        [ForeignKey("MaterailId")]
        public Materails Materail { get; set; }

        public string MaterailId { get;  set; }

        [ForeignKey("StudentId")]
        public AppUser Student { get; set; }

        public string StudentId { get;  set; }
    }
}
