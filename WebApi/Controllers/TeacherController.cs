using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.TeacherDTO;
using WebApi.DTO.UserDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TeacherController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<Courses> coursesUnitOfWork;
        private readonly IUnitOfWork<TypeJop> jTUnitOfWork;

        public TeacherController(RoleManager<IdentityRole> roleManager,Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting, UserManager<AppUser> userManager,IUnitOfWork<AppUser> UserUnitOfWork , IUnitOfWork<Courses> coursesUnitOfWork , IUnitOfWork<TypeJop> JTUnitOfWork)
        {
            this.roleManager = roleManager;
            this.hosting = hosting;
            this.userManager = userManager;
            userUnitOfWork = UserUnitOfWork;
            this.coursesUnitOfWork = coursesUnitOfWork;
            jTUnitOfWork = JTUnitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult>GetTeachers()
        {
            var jTT = jTUnitOfWork.Entity.Find(x => x.JopType.Contains("Teacher"));
            if (jTT == null)
                return NotFound();
            var teacher = await userUnitOfWork.Entity.FindAll(x => x.TypeJopId == jTT.Id);
            if (teacher.Count() == 0)
                return NotFound();




            var model = teacher.Select(t => new GetTeacherDTO
            {
                teacherId = t.Id,
                teacherName = t.Name,
                courseName = coursesUnitOfWork.Entity.FindAll(x => x.TeacherId == t.Id).Result.Select(c=>c.CourseName)

            });

            return Ok(model);


        }

        [HttpGet("{teacherId}")]
        public async Task<IActionResult> GetTeacherById(string teacherId)
        {
            var teacher = await userUnitOfWork.Entity.GetAsync(teacherId);
            if (teacher == null) return NotFound();
            var model = new GetTeacherDTO
            {
                teacherId = teacher.Id,
                teacherName = teacher.Name,
                courseName = coursesUnitOfWork.Entity.FindAll(x => x.TeacherId == teacher.Id).Result.Select(c => c.CourseName)

            };
            return Ok(model);
        }

        [HttpPost]
        [Authorize("AdminRole")]
        public async Task<IActionResult> AddTeacher([FromForm] AddTeacherDTO dto)
        {
            var errors = ModelState.Values.SelectMany(r => r.Errors);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var validuserbyNationa = await userManager.FindByNameAsync(dto.NationaNumber);

            if (validuserbyNationa != null)
            {
                ModelState.AddModelError("NationaNumber", "NationalNumber Is already exists");
                return BadRequest(ModelState);


            }

            var sutdId = jTUnitOfWork.Entity.Find(x => x.JopType.Contains("Teacher"));

            var user = new AppUser
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                TypeJopId = sutdId.Id,
                UserName = dto.NationaNumber,
                CreatedAt = DateTime.Now,
                Photo = "Photos/149071.png",
            };

            var result = await userManager.CreateAsync(user, dto.NationaNumber + "Abcd123#");

            if (result.Succeeded)
            {
                var Claim = new System.Security.Claims.Claim("User", "User");
                await userManager.AddClaimAsync(user, Claim);

                var roleIsExists = await roleManager.RoleExistsAsync("Teacher");
                if (roleIsExists)
                {
                    await userManager.AddToRoleAsync(user, "Teacher");
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole("Teacher"));
                    await userManager.AddToRoleAsync(user, "Teacher");
                }


                

            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            userUnitOfWork.Save();
            return Ok();

        }


        
        [HttpPut("{teacherId}")]
        [Authorize("TeacherRole")]
        public async Task<IActionResult> EditUser([FromForm] UpdateTeacherDTO dto, string teacherId)
        {
            var user = await userManager.FindByIdAsync(teacherId);
            if (user == null)
                return NotFound(ModelState);

            if (dto.Photo != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Photos/");
                string fullPath = Path.Combine(uploads, dto.Photo.FileName);
                dto.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));

                user.Photo = dto.Photo.FileName;

            }

            user.Name = dto.Name;
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            user.Email = dto.Email;
            user.Gender = dto.Gender;
            user.UserName = dto.UserName;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            jTUnitOfWork.Save();
            return Ok("User Is Updated");


        }


        [HttpDelete("{Id}")]
        [Authorize("AdminRole")]
        public async Task<IActionResult> Delete(string Id)
        {
            var Teacher = await userManager.FindByIdAsync(Id);
            if (Teacher == null) return NotFound(ModelState);

            await userManager.DeleteAsync(Teacher);

            return Ok("The User Is Deleted");



        }
    }
}
