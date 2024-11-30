using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.AttendanceDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("TeacherRole")]
    public class AttendanceController : ControllerBase
    {
        private readonly IUnitOfWork<Attendance> attendanceUnitOfWork;
        private readonly IUnitOfWork<Enrollment> enrollmentUnitOfWork;
        private readonly IUnitOfWork<AppUser> userunitOfWork;
        private readonly IUnitOfWork<Materails> materailunitOfWork;
        private readonly IUnitOfWork<Courses> coursesunitOfWork;

        public AttendanceController(IUnitOfWork<Attendance> AttendanceUnitOfWork, IUnitOfWork<Enrollment> EnrollmentUnitOfWork, IUnitOfWork<AppUser> UserunitOfWork , IUnitOfWork<Materails> MaterailunitOfWork, IUnitOfWork<Courses> CoursesunitOfWork)
        {
            attendanceUnitOfWork = AttendanceUnitOfWork;
            enrollmentUnitOfWork = EnrollmentUnitOfWork;
            userunitOfWork = UserunitOfWork;
            materailunitOfWork = MaterailunitOfWork;
            coursesunitOfWork = CoursesunitOfWork;
        }

        [HttpGet("GetAttendance/{courseId}")]
        public async Task<IActionResult> GetAttendance(string courseId)
        {
            var course =await coursesunitOfWork.Entity.GetAsync(courseId);


            var materail =  materailunitOfWork.Entity.Find(x=>x.CourseId== courseId);

            var attend = await attendanceUnitOfWork.Entity.FindAll(x=>x.AttendDate.Day >= DateTime.Now.Day && x.AttendDate.Day <= DateTime.Now.AddDays(3).Day);

            if (attend.Count() == 0)
                return NotFound();


            //var enroll = await enrollmentUnitOfWork.Entity.FindAll(x => x.courseId == course.CourseId);

            var std = userunitOfWork.Entity.Find(x => attend.Select(s => s.StudentId).Contains(x.Id));

            var model = attend.Select(x => new GetAttendanceDTO
            {
                attendID = x.AttendId,
                CourseName = course.CourseName,
                materailName = materail.materailName,
                stdName = std.Name,
                UserName = std.UserName,


            });

            return Ok(model);

            

        }

        [HttpGet("GetAbsence/{courseId}")]
        public async Task<IActionResult> GetAbsence(string courseId)
        {
            var attend = await attendanceUnitOfWork.Entity.FindAll(x => x.AttendDate.Day >= DateTime.Now.Day && x.AttendDate.Day <= DateTime.Now.AddDays(3).Day);

            if (attend.Count() == 0)
                return NotFound();

            var course = await coursesunitOfWork.Entity.GetAsync(courseId);
            var materail = materailunitOfWork.Entity.Find(x => x.CourseId == courseId);

            var enroll = await enrollmentUnitOfWork.Entity.FindAll(x => x.courseId == course.CourseId);

            var Absence = enroll.SkipWhile(x => attend.Select(x => x.StudentId).Contains(x.studentId) && attend.Select(x => x.CourseId).Contains(x.courseId)).Select(x => x.studentId);

            var std = userunitOfWork.Entity.Find(x => Absence.Contains(x.Id));

             if(std == null)
                return NotFound("EveryBody Attend");

            var model = attend.Select(x => new GetAttendanceDTO
            {
                attendID = x.AttendId,
                CourseName = course.CourseName,
                materailName = materail.materailName,
                stdName = std.Name,
                UserName = std.UserName,

            });

            return Ok(model);



        }

        [HttpPost]
        public async Task<IActionResult> AddAttendance(string courseId, [FromForm] AddAttendanceDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var courses =  coursesunitOfWork.Entity.Find(x => x.CourseId == courseId);

            var std = userunitOfWork.Entity.Find(x => x.UserName == dto.UserName || x.Name == dto.UserName);

            if (courses == null)
                return NotFound();

            var attend = new Attendance
            {
                AttendId = Guid.NewGuid().ToString(),
                CourseId = courseId,
                StudentId = std.Id,
                AttendDate = DateTime.Now,
            };

            await attendanceUnitOfWork.Entity.AddAsync(attend);
            attendanceUnitOfWork.Save();
            return Ok();








        }


        [HttpDelete]
        public  IActionResult DeleteAttendance(string courseId , string student)
        {
            var Attend = attendanceUnitOfWork.Entity.Find(x => x.CourseId == courseId && x.StudentId == student);
            if (Attend == null)
                return NotFound(ModelState);

            attendanceUnitOfWork.Entity.Delete(Attend);
            attendanceUnitOfWork.Save();
            return Ok();

        }



    }
}
