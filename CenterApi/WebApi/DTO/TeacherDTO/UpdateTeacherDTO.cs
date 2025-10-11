using WebApi.DTO.UserDTO;

namespace WebApi.DTO.TeacherDTO
{
    public class UpdateTeacherDTO : GetUserDTO
    {
        public IFormFile Photo { get; set; }
    }
}
