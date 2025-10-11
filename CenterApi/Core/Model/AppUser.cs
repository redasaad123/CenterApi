using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    
    public class AppUser : IdentityUser
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = " Name")]
        public string Name { get; set; }
        [ForeignKey("TypeJopId")]
        
        public TypeJop TypeJop { get; set; }
        public string TypeJopId { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumberParent { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Photo { get; set; }
        public DateTime CreatedAt { get; set; }




    }
}
