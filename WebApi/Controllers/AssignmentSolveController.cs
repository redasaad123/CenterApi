using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.SolveAssignmentDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentSolveController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<Tasks> taskUnitOfWork;
        private readonly IUnitOfWork<SolveTask> SolveTaskUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;

        public AssignmentSolveController(IUnitOfWork<AppUser> UserUnitOfWork, IUnitOfWork<Tasks> TaskUnitOfWork, IUnitOfWork<Materails> SolveTaskUnitOfWork, UserManager<AppUser> userManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            userUnitOfWork = UserUnitOfWork;
            taskUnitOfWork = TaskUnitOfWork;
            SolveTaskUnitOfWork = SolveTaskUnitOfWork;
            this.userManager = userManager;
            this.hosting = hosting;
        }

        [HttpGet("{taskId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> GetSolveAssignment(string taskId)
        {
            var Tsolve = await SolveTaskUnitOfWork.Entity.FindAll(x => x.taskId == taskId);
            if (Tsolve.Count() == 0)
                return NotFound();

            var model = Tsolve.Select(t => new GetSolveAssignmentDTO
            {
                SolvePdf = t.solveURl,
                SolvedAt = t.SolvedAt,
                solveTaskId = t.solveTaskId,
                studentId = t.studentId,
                studentName = userUnitOfWork.Entity.Find(x => x.Id == t.studentId).Name,
                TaskName = taskUnitOfWork.Entity.Find(x => x.TaskId == taskId).TaskName,
                TaskId = taskId,
            });

            return Ok(model);



        }

        [HttpGet("{taskId}/{studentId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> GetSolveForSTD(string studentId, string taskId)
        {
            var solve = SolveTaskUnitOfWork.Entity.Find(x => x.studentId == studentId && x.taskId == taskId);
            if (solve == null)
                return NotFound();

            var model = new GetSolveAssignmentDTO
            {
                SolvePdf = solve.solveURl,
                SolvedAt = solve.SolvedAt,
                solveTaskId = solve.solveTaskId,
                studentId = solve.studentId,
                studentName = userUnitOfWork.Entity.Find(x => x.Id == solve.studentId).Name,
                TaskName = taskUnitOfWork.Entity.Find(x => x.TaskId == taskId).TaskName,
                TaskId = taskId,

            };
            return Ok(model);


        }

        [HttpPost("{taskId}")]
        [Authorize("StudentRole")]
        public async Task<IActionResult> AddSolve([FromForm] AddSolveDTO dto, string taskId)
        {
            if (ModelState.IsValid)
                return BadRequest(ModelState);

            var std = userManager.GetUserId(HttpContext.User);

            if (dto.SolvePdf != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Solve/");
                string fullPath = Path.Combine(uploads, dto.SolvePdf.FileName);
                dto.SolvePdf.CopyTo(new FileStream(fullPath, FileMode.Create));


            }

            var solve = new SolveTask
            {
                solveTaskId = Guid.NewGuid().ToString(),
                SolvedAt = DateTime.Now,
                solveURl = dto.SolvePdf.FileName,
                studentId = std,
                taskId = taskId,
            };
            await SolveTaskUnitOfWork.Entity.AddAsync(solve);
            SolveTaskUnitOfWork.Save();
            return Ok(solve);

        }

        [HttpPut("{TSolveId}")]
        [Authorize("StudentRole")]

        public async Task<IActionResult> UpdateTask(string TSolveId , [FromForm ] AddSolveDTO dto)
        {
            var solve = SolveTaskUnitOfWork.Entity.Find(x => x.solveTaskId == TSolveId);
            if (solve == null)
                return NotFound(ModelState);

            if (dto.SolvePdf != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Solve/");
                string fullPath = Path.Combine(uploads, dto.SolvePdf.FileName);
                dto.SolvePdf.CopyTo(new FileStream(fullPath, FileMode.Create));

                solve.solveURl = dto.SolvePdf.FileName;

            }

            await  SolveTaskUnitOfWork.Entity.UpdateAsync(solve);

            SolveTaskUnitOfWork.Save();
            return Ok();




        }

        [HttpDelete]
        [Authorize("StudentRole")]
        public IActionResult DeleteSolveTask(string TSolveId)
        {
            var solve = SolveTaskUnitOfWork.Entity.Find(x => x.solveTaskId == TSolveId);
            if (solve == null)
                return NotFound(ModelState);
             SolveTaskUnitOfWork.Entity.Delete(solve);

            SolveTaskUnitOfWork.Save();
            return Ok();


        }







    }
}
