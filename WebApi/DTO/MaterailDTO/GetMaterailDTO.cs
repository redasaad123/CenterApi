using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DTO.MaterailDTO
{
    public class GetMaterailDTO
    {
        public string materailId { get; set; }

        public string materailName { get; set; }

        public string materailVideo { get; set; }

        public string materailPdf { get; set; }

        public string CourseName { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
