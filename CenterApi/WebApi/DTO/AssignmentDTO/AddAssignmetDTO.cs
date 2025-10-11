using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.DTO.AssignmentDTO
{
    public class AddAssignmetDTO
    {
        public string TaskName { get; set; }

        public string Time { get; set; }

        public IFormFile UrlTask { get; set; }

    }
}
