using System.ComponentModel.DataAnnotations;
using WebApi.DTO.AuthenticateDTO;

namespace WebApi.DTO.UserDTO
{
    public class GetUserDTO 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhotoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string? PhoneNumberParent { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string joptype { get; set; }
    }
}
