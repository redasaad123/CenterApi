using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.CoursesDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CoursesController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<Courses> coursUnitOfWork;

        public CoursesController(UserManager<AppUser> userManager, IUnitOfWork<AppUser> UserUnitOfWork, IUnitOfWork<Courses> CoursUnitOfWork)
        {
            this.userManager = userManager;
            userUnitOfWork = UserUnitOfWork;
            coursUnitOfWork = CoursUnitOfWork;
        }

        [HttpGet("Courses")]
        public async Task<IActionResult> Courses()
        {

            var model = await coursUnitOfWork.Entity.FindAll(x => new GetCourseDto
            {
                CourseId = x.CourseId,
                CourseName = x.CourseName,
                Price = x.Price,
                CreatedAt = x.CreatedAt,
                TeacherDescription = x.TeacherDescription,
                Teacher = userManager.Users.Where(u => u.Id == x.TeacherId).Select(x => x.Name).FirstOrDefault()

            });

            return Ok(model);

        }

        [HttpGet("{TeacherName}")]
        public async Task<IActionResult> CoursesByTeacherName(string TeacherName)
        {
            var validtracher = userUnitOfWork.Entity.Find(x => x.Name == TeacherName);

            if (validtracher == null)
                return NotFound("Teacher Name Is Not Found");

            var model = await coursUnitOfWork.Entity.FindAll(x => x.TeacherId == validtracher.Id, course => new GetCourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Price = course.Price,
                CreatedAt = course.CreatedAt,
                TeacherDescription = course.TeacherDescription,
                Teacher = TeacherName

            });

            if (model.Count > 0)
            {
                return Ok(model);
            }
            else
            {
                return NotFound($"This Teacher {TeacherName} Does Not Have a Courses ");
            }




        }

        [HttpGet("GetCourseById/{CourseId}")]
        public async Task<IActionResult> GetCourseById(string CourseId)
        {
            var course = await coursUnitOfWork.Entity.GetAsync(CourseId);
            if (course == null)
                return NotFound("This Course Not Found ");

            var model = await coursUnitOfWork.Entity.Mapping(x => new GetCourseDto
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Price = course.Price,
                CreatedAt = course.CreatedAt,
                TeacherDescription = course.TeacherDescription,
                Teacher = userManager.Users.Where(u => u.Id == x.TeacherId).Select(x => x.Name).FirstOrDefault()


            });

            return Ok(model);

        }

        [HttpPost]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> AddCourse([FromForm] AddCourseDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var teacherId = userUnitOfWork.Entity.Find(x => x.Name == dto.Teacher);
            if (teacherId == null)
            {
                return NotFound($"This Teacher {dto.Teacher} Not Found");
            }

            var couses = await coursUnitOfWork.Entity.FindAll(x => x.TeacherId == teacherId.Id);

            if (couses.Select(x => x.CourseName).Contains(dto.CourseName))
            {
                ModelState.AddModelError("CourseName", $"This Teacher {dto.CourseName} actually owns this course");

                return BadRequest(ModelState);  

            }

            var course = new Courses
            {
                CourseId = Guid.NewGuid().ToString(),
                CourseName = dto.CourseName,
                CreatedAt = DateTime.Now,
                Price = dto.Price,
                TeacherId = teacherId.Id,
                TeacherDescription = dto.TeacherDescription,
            };

            await coursUnitOfWork.Entity.AddAsync(course);
            coursUnitOfWork.Save();



            return Ok(course);

        }

        [HttpPut("{courseId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> UpdateCourse([FromForm]AddCourseDTO dto, string courseId)
        {
            var course = await coursUnitOfWork.Entity.GetAsync(courseId);

            if (course == null)
                return NotFound("This Course Is Not Found");

            course.CourseName = dto.CourseName;
            course.Price = dto.Price;
            course.TeacherDescription = dto.TeacherDescription;

            var teacher = userUnitOfWork.Entity.Find(x => x.Name == dto.Teacher);
            course.TeacherId = teacher.Id;

            await coursUnitOfWork.Entity.UpdateAsync(course);

            return Ok(course);


        }

        [HttpDelete("{CourseId}")]
        [Authorize("TeacherRole")]
        public async Task< IActionResult> DeleteCourse(string CourseId)
        {
            var course = await coursUnitOfWork.Entity.GetAsync(CourseId);

            if (course == null)
                return NotFound("This Course Not Found");

            coursUnitOfWork.Entity.Delete(course);
            coursUnitOfWork.Save();

            return Ok();
            




        }


    }
}
