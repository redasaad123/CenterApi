using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.GradesDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class GradesController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userunitOfWork;
        private readonly IUnitOfWork<Grades> gradesUnitOfWork;
        private readonly IUnitOfWork<Courses> coursesUnitOfWork;

        public GradesController(IUnitOfWork<AppUser> UserunitOfWork, IUnitOfWork<Grades> GradesUnitOfWork, IUnitOfWork<Courses> CoursesUnitOfWork)
        {
            userunitOfWork = UserunitOfWork;
            gradesUnitOfWork = GradesUnitOfWork;
            coursesUnitOfWork = CoursesUnitOfWork;
        }


        [HttpGet]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> GetGrades()
        {
            var grades = await gradesUnitOfWork.Entity.GetAllAsync();
            if (grades.Count() == 0)
                return NotFound();

            var model = grades.Select(g => new GetGradesDTO
            {
                GradeId = g.gradeId,
                CourseName = coursesUnitOfWork.Entity.GetAsync(g.CourseId).Result.CourseName,
                StudentName = userunitOfWork.Entity.Find(x => x.Id == g.StudentId).Name,
                grade = g.grade,

            });

            return Ok(model);

        }

        [HttpGet("GradesCourses/{courseId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> GetGradesForeachCourses(string courseId)
        {
            var grades = await gradesUnitOfWork.Entity.FindAll(x => x.CourseId == courseId);
            if (grades.Count() == 0)
                return NotFound();

            var model = grades.Select(g => new GetGradesDTO
            {
                GradeId = g.gradeId,
                CourseName = coursesUnitOfWork.Entity.GetAsync(g.CourseId).Result.CourseName,
                StudentName = userunitOfWork.Entity.Find(x => x.Id == g.StudentId).Name,
                grade = g.grade,

            });


            return Ok(model);

        }

        [HttpGet("GradesStudent/{studentId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<IActionResult> GetgradesForeachStudent(string studentId)
        {
            var grades = await gradesUnitOfWork.Entity.FindAll(x => x.StudentId == studentId);
            if (grades.Count() == 0)
                return NotFound();

            var model = grades.Select(g => new GetGradesDTO
            {
                GradeId = g.gradeId,
                CourseName = coursesUnitOfWork.Entity.GetAsync(g.CourseId).Result.CourseName,
                StudentName = userunitOfWork.Entity.Find(x => x.Id == g.StudentId).Name,
                grade = g.grade,

            });


            return Ok(model);
        }

        [HttpGet("GradesStudentInCourse/{studentId}/{courseId}")]
        [Authorize(Roles = "Teacher,Student")]
        public async Task<IActionResult> GetGradesStudentInCourse(string studentId, string courseId)
        {
            var grades = await gradesUnitOfWork.Entity.FindAll(x => x.StudentId == studentId && x.CourseId == courseId);
            if (grades.Count() == 0)
                return NotFound();

            var model = grades.Select(g => new GetGradesDTO
            {
                GradeId = g.gradeId,
                CourseName = coursesUnitOfWork.Entity.GetAsync(g.CourseId).Result.CourseName,
                StudentName = userunitOfWork.Entity.Find(x => x.Id == g.StudentId).Name,
                grade = g.grade,

            });
            return Ok(model);
        }

        [HttpPost("{courseId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> AddGrade(string courseId, [FromForm] AddGradesDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var course = await coursesUnitOfWork.Entity.GetAsync(courseId);
            if (course == null)
                return NotFound();
            var grade = new Grades
            {
                gradeId = Guid.NewGuid().ToString(),
                StudentId = userunitOfWork.Entity.Find(x => x.Name == dto.StudentName).Id,
                CourseId = courseId,
                grade = dto.grade,
            };

            await gradesUnitOfWork.Entity.AddAsync(grade);
            gradesUnitOfWork.Save();
            return Ok(grade);

        }
        [HttpPut]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> UpdateGrade(string GradeId, [FromForm] UpdateGradeDTO dto)
        {
            var grade = await gradesUnitOfWork.Entity.GetAsync(GradeId);
            if (grade == null)
                return NotFound();

            grade.grade = dto.grade;

            await gradesUnitOfWork.Entity.UpdateAsync(grade);
            gradesUnitOfWork.Save();
            return Ok(grade);



        }

        [HttpDelete]
        [Authorize("TeacherRole")]
        public async Task< IActionResult >DeleteGrade(string GradeId)
        {
            var grade = await gradesUnitOfWork.Entity.GetAsync(GradeId);
            if (grade == null)
                return NotFound();

            gradesUnitOfWork.Entity.Delete(grade);
            gradesUnitOfWork.Save();
            return Ok();
        }










    }
}
