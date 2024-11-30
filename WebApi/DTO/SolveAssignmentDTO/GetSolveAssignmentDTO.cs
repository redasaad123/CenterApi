using Core.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.DTO.SolveAssignmentDTO
{
    public class GetSolveAssignmentDTO
    {
        public string TaskId { get; set; }
        public string TaskName { get; set; }

        public string solveTaskId { get; set; }

        public DateTime SolvedAt { get; set; }

        public string SolvePdf { get; set; }

        public string studentId { get; set; }

        public string studentName { get; set; }



    }
}
