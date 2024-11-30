using System.ComponentModel.DataAnnotations;
using WebApi.DTO.AuthenticateDTO;

namespace WebApi.DTO.UserDTO
{
    public class AddUserDTO 
    {
        public string NationaNumber { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumberParent { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }



    }
}
