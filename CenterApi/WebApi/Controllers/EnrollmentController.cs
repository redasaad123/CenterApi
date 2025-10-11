using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Linq;
using WebApi.DTO.Enrollment;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher,Admin")]
    public class EnrollmentController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<Courses> courseUnitOfWork;
        private readonly IUnitOfWork<Enrollment> enrollmentunitOfWork;

        public EnrollmentController(IUnitOfWork<AppUser> usreUnitOfWork, IUnitOfWork<Courses> courseUnitOfWork, IUnitOfWork<Enrollment> EnrollmentunitOfWork)
        {
            this.userUnitOfWork = usreUnitOfWork;
            this.courseUnitOfWork = courseUnitOfWork;
            enrollmentunitOfWork = EnrollmentunitOfWork;
        }

        [HttpGet("{month:int}")]
        public async Task<IActionResult> GetEnrollments(int month)
        {
            var Enrollment = await enrollmentunitOfWork.Entity.FindAll(x => x.enrolledAt.Month == month);
            if (Enrollment.Count() ==0)
                return NotFound("Not Found Any Enrollment This month");

            var coures =  courseUnitOfWork.Entity.Find(x => Enrollment.Select(x => x.courseId).Contains(x.CourseId));

            var teacher = userUnitOfWork.Entity.Find(x => x.Id == coures.TeacherId);

            var std =  userUnitOfWork.Entity.Find(x => Enrollment.Select(x =>x.studentId).Contains(x.Id));


            var model = await enrollmentunitOfWork.Entity.Mapping  ( Enrol => new GetEnrollmentsDTO
            {
                CourseId = coures.CourseId,
                CourseName = coures.CourseName,
                stdName = std.Name,
                stdId = std.Id,
                UserName=std.UserName,
                TeacherName = teacher.Name,
                TeacherId = teacher.Id,


            });
            return Ok(model);

        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetEnrollByIdCourse(string courseId)
        {
            var Enrollment = await enrollmentunitOfWork.Entity.FindAll(x => x.courseId == courseId );
            if (Enrollment.Count() == 0)
                return NotFound("Not Found Any Enrollment This month");

            var coures = courseUnitOfWork.Entity.Find(x => Enrollment.Select(x => x.courseId).Contains(x.CourseId));

            var teacher = userUnitOfWork.Entity.Find(x => x.Id == coures.TeacherId);

            var std = userUnitOfWork.Entity.Find(x => Enrollment.Select(x => x.studentId).Contains(x.Id));


            var model = await enrollmentunitOfWork.Entity.Mapping(Enrol => new GetEnrollmentsDTO
            {
                CourseId = coures.CourseId,
                CourseName = coures.CourseName,
                stdName = std.Name,
                stdId = std.Id,
                UserName = std.UserName,
                TeacherName = teacher.Name,
                TeacherId = teacher.Id,


            });
            return Ok(model);

        }





        [HttpPost("{CourseId}")]
        public async Task<IActionResult> AddEnrollment([FromForm] AddEnrollmentDTO dto , string CourseId)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            var std = userUnitOfWork.Entity.Find(x => x.Name == dto.stdName);
            if(std == null)
                return NotFound();


            var enroll = new Enrollment
            {
                EnrollmentId = Guid.NewGuid().ToString(),
                enrolledAt = DateTime.Now,
                courseId = CourseId,
                studentId = std.Id,
            };

            await enrollmentunitOfWork.Entity.AddAsync(enroll);
            enrollmentunitOfWork.Save();
            return Ok(enroll);

            

        }

        [HttpDelete]
        public async Task< IActionResult> Delete(string enrollId)
        {
            var enrollment = await enrollmentunitOfWork.Entity.GetAsync(enrollId);
            if (enrollment == null)
                return NotFound();
            enrollmentunitOfWork.Entity.Delete(enrollment);
            enrollmentunitOfWork.Save();
            return Ok("The Enrollment Is Deleted ");
        }


    }
}
