using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.AuthenticateDTO
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string PasswordConfirmed { get; set; }
        public IFormFile? Photo { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumberParent { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
    }
}
