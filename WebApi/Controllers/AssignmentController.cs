using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.AssignmentDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AssignmentController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<Tasks> taskUnitOfWork;
        private readonly IUnitOfWork<Materails> materailsUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;

        public AssignmentController(IUnitOfWork<AppUser> UserUnitOfWork, IUnitOfWork<Tasks> TaskUnitOfWork, IUnitOfWork<Materails> MaterailsUnitOfWork, UserManager<AppUser> userManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            userUnitOfWork = UserUnitOfWork;
            taskUnitOfWork = TaskUnitOfWork;
            materailsUnitOfWork = MaterailsUnitOfWork;
            this.userManager = userManager;
            this.hosting = hosting;
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks(string courseId)
        {
            var tasks = await taskUnitOfWork.Entity.GetAllAsync();
            if (tasks.Count() == 0)
                return NotFound();
            return Ok(tasks);

        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetTasksByCourse(string courseId)
        {

            var tasks = await taskUnitOfWork.Entity.FindAll(x => x.CourseId == courseId);
            if (tasks.Count() == 0)
                return NotFound();
            return Ok(tasks);

        }

        [HttpGet("GetTaskBtId/{taskId}")]
        public async Task<IActionResult> GetTaskBtId(string taskId)
        {
            var task = taskUnitOfWork.Entity.Find(x => x.TaskId == taskId);
            if (task == null) return NotFound();
            return Ok(task);

        }



        [HttpPost("{courseId}")]
        public async Task<IActionResult> AddTask(string courseId, [FromForm] AddAssignmetDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            if (dto.UrlTask != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Assignments/");
                string fullPath = Path.Combine(uploads, dto.UrlTask.FileName);
                dto.UrlTask.CopyTo(new FileStream(fullPath, FileMode.Create));
            }

            var task = new Tasks
            {
                TaskId = Guid.NewGuid().ToString(),
                CourseId = courseId,
                TaskName = dto.TaskName,
                Time = dto.Time,
                DateTask = DateTime.Now,
                UrlTask = dto.UrlTask.FileName,


            };

            await taskUnitOfWork.Entity.AddAsync(task);
            taskUnitOfWork.Save();
            return Ok(task);



        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(string taskId, [FromForm] UpdateTaskDTO dto)
        {
            var task = taskUnitOfWork.Entity.Find(x => x.TaskId == taskId);
            if (task == null)
                return NotFound();

            if (dto.UrlTask != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Assignments/");
                string fullPath = Path.Combine(uploads, dto.UrlTask.FileName);
                dto.UrlTask.CopyTo(new FileStream(fullPath, FileMode.Create));

                task.UrlTask = dto.UrlTask.FileName;
            }

            task.Time = dto.Time;
            task.TaskName = dto.TaskName;

            await taskUnitOfWork.Entity.UpdateAsync(task);
            taskUnitOfWork.Save();
            return Ok(task);


        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteTask(string taskId)
        {
            var task = taskUnitOfWork.Entity.Find(x => x.TaskId == taskId);
            if (task == null)
                return NotFound();

            taskUnitOfWork.Entity.Delete(task);
            taskUnitOfWork.Save();
            return Ok();

        }








    }
}
