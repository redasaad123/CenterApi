namespace WebApi.DTO.UserDTO
{
    public class UpdateUserDto : GetUserDTO
    {
        public IFormFile Photo { get; set; }
    }
}
