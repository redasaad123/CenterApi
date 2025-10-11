using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.MaterailDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterailsController : ControllerBase
    {
        private readonly IUnitOfWork<Materails> materailUnitOfWork;
        private readonly IUnitOfWork<Courses> courseUnitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;

        public MaterailsController(IUnitOfWork<Materails> MaterailUnitOfWork, IUnitOfWork<Courses> courseUnitOfWork, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting)
        {
            materailUnitOfWork = MaterailUnitOfWork;
            this.courseUnitOfWork = courseUnitOfWork;
            this.hosting = hosting;
        }

        [HttpGet("{courseId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<IActionResult> GetMaterails(string courseId)
        {
            var course = courseUnitOfWork.Entity.GetAsync(courseId);

            var materail = await materailUnitOfWork.Entity.FindAll(x => x.CourseId == courseId, mater => new GetMaterailDTO
            {
                materailId = mater.materailId,
                materailName = mater.materailName,
                materailVideo = mater.materailVideo,
                materailPdf = mater.materailPdf,
                UploadedAt = mater.UploadedAt,
                CourseName = course.Result.CourseName,
            });
            if (materail.Count() == 0)
                return NotFound("This Course hasn't Any Materails");

            return Ok(materail);

        }


        [HttpPost("{courseId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> AddMaterail(string courseId, [FromForm] AddMaterailDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var validcourse = await courseUnitOfWork.Entity.GetAsync(courseId);
                if (validcourse == null)
                    return NotFound("This Coures Not Found");

                if (dto.materailVideo != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, @"VideosMaterails/");
                    string fullPath = Path.Combine(uploads, dto.materailVideo.FileName);
                    dto.materailVideo.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                


                if (dto.materailPdf != null)
                {
                    string uploads = Path.Combine(hosting.WebRootPath, @"PdfMaterails/");
                    string fullPath = Path.Combine(uploads, dto.materailPdf.FileName);
                    dto.materailPdf.CopyTo(new FileStream(fullPath, FileMode.Create));
                }
                

                var materail = new Materails
                {
                    materailId = Guid.NewGuid().ToString(),
                    materailName = dto.materailName,
                    materailVideo = dto.materailVideo.FileName,
                    materailPdf =  dto.materailPdf.FileName,
                    UploadedAt = DateTime.Now,
                    CourseId = courseId,
                };

                await materailUnitOfWork.Entity.AddAsync(materail);
                materailUnitOfWork.Save();
                return Ok();

            }
            catch
            {
                return BadRequest(ModelState);
            }
            

        }

        [HttpPut("{materailId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> UpdateMaterail(string materailId ,[FromForm] UpdateMaterailDTO dto)
        {
            var materail = await materailUnitOfWork.Entity.GetAsync(materailId);

            if (materail == null)
                return NotFound("This Matetails Not Found");

            if (dto.materailVideo != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"VideosMaterails/");
                string fullPath = Path.Combine(uploads, dto.materailVideo.FileName);
                dto.materailVideo.CopyTo(new FileStream(fullPath, FileMode.Create));

                materail.materailVideo = dto.materailVideo.FileName;

            }



            if (dto.materailPdf != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"PdfMaterails/");
                string fullPath = Path.Combine(uploads, dto.materailPdf.FileName);
                dto.materailPdf.CopyTo(new FileStream(fullPath, FileMode.Create));

                materail.materailPdf = dto.materailPdf.FileName;
            }

            materail.materailName = dto.materailName;

            await materailUnitOfWork.Entity.UpdateAsync(materail);

            materailUnitOfWork.Save();

            return Ok(materail);

        }


        [HttpDelete]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> DeleteMaterail(string materailId)
        {
            var materail = await materailUnitOfWork.Entity.GetAsync(materailId);
            if (materail == null)
                return NotFound("This Materail Not Found");
             materailUnitOfWork.Entity.Delete(materail);
            materailUnitOfWork.Save();
            return Ok("Materail Is Deleted");

        }


    }
}
