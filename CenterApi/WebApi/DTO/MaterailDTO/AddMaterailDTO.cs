namespace WebApi.DTO.MaterailDTO
{
    public class AddMaterailDTO
    {

        public string materailName { get; set; }

        public IFormFile? materailVideo { get; set; }

        public IFormFile? materailPdf { get; set; }

    }
}
