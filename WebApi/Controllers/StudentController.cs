using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.WebSockets;
using WebApi.DTO.UserDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork<Enrollment> enrollmentUnitOfWork;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork<AppUser> userUnitOfWord;
        private readonly IUnitOfWork<TypeJop> jTUnitOfWord;

        public StudentController(RoleManager<IdentityRole> roleManager,IUnitOfWork<Enrollment> EnrollmentUnitOfWork, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting, UserManager<AppUser> userManager, IUnitOfWork<AppUser> UserUnitOfWord, IUnitOfWork<TypeJop> JTUnitOfWord)
        {
            this.roleManager = roleManager;
            enrollmentUnitOfWork = EnrollmentUnitOfWork;
            this.hosting = hosting;
            this.userManager = userManager;
            userUnitOfWord = UserUnitOfWord;
            jTUnitOfWord = JTUnitOfWord;
        }

        [HttpGet("GetStudents")]
        [Authorize(Roles ="Teacher,Admin")]
        public async Task<IActionResult> GetStudents()
        {
            var JTSID = jTUnitOfWord.Entity.Find(x => x.JopType.Contains("Student"));

            var stds = await userUnitOfWord.Entity.FindAll(x => x.TypeJopId == JTSID.Id, std => new GetUserDTO
            {
                Id = std.Id,
                Name = std.Name,
                PhoneNumber = std.PhoneNumber,
                Address = std.Address,
                Email = std.Email,
                UserName = std.UserName,
                Gender = std.Gender,
                PhotoUrl = std.Photo,
                PhoneNumberParent = std.PhoneNumberParent,
                joptype = JTSID.JopType,
            });

            if (stds == null)
                return NotFound();

            return Ok(stds);

        }

        [HttpGet("{Id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetStudentById(string Id)
        {
            var std = await userManager.FindByIdAsync(Id);
            if (std == null) return NotFound();
            var JTSID = jTUnitOfWord.Entity.Find(x => x.JopType.Contains("Student"));

            var model = await userUnitOfWord.Entity.FindAll(x => x.Id == Id, st => new GetUserDTO
            {
                Id = st.Id,
                Name = st.Name,
                PhoneNumber = st.PhoneNumber,
                Address = st.Address,
                Email = st.Email,
                UserName = st.UserName,
                Gender = st.Gender,
                PhotoUrl = st.Photo,
                PhoneNumberParent = st.PhoneNumberParent,
                joptype = JTSID.JopType,

            });


            return Ok(model);



        }
        [HttpGet("StudentInCourse/{CourseId}")]
        [Authorize("TeacherRole")]
        public async Task <IActionResult> GetStudentInCourse(string CourseId)
        {
            var STDs = await enrollmentUnitOfWork.Entity.FindAll(x=>x.courseId == CourseId);
            if (STDs.Count() == 0) return NotFound();

            var std = userUnitOfWord.Entity.Find(x => STDs.Select(x => x.studentId).Contains(x.Id));

            var model = new GetUserDTO
            {
                Id = std.Id,
                Name = std.Name,
                PhoneNumber = std.PhoneNumber,
                Address = std.Address,
                Email = std.Email,
                UserName = std.UserName,
                Gender = std.Gender,
                PhotoUrl = std.Photo,
                PhoneNumberParent = std.PhoneNumberParent,

            };

            return Ok(model);




        }
        

        [HttpPost("AddStudent")]
        [Authorize(Roles = "Teacher,Admin")]

        public async Task<IActionResult> AddStudent([FromForm] AddUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var validuserbyNationa = await userManager.FindByIdAsync(dto.NationaNumber);

            if (validuserbyNationa != null) return BadRequest(ModelState);


            var JTSID = jTUnitOfWord.Entity.Find(x => x.JopType.Contains("Student"));

            var std = new AppUser
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                TypeJopId = JTSID.Id,
                UserName = dto.NationaNumber,
                CreatedAt = DateTime.Now,
                Photo = "Photos/149071.png"
            };
            var result = await userManager.CreateAsync(std, dto.NationaNumber + "Abcd123#");

            if (result.Succeeded)
            {
                var Claim = new System.Security.Claims.Claim("User", "User");
                await userManager.AddClaimAsync(std, Claim);

                var roleIsExists = await roleManager.RoleExistsAsync("Teacher");
                if (roleIsExists)
                {
                    await userManager.AddToRoleAsync(std, "Student");
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole("Student"));
                    await userManager.AddToRoleAsync(std, "Student");
                }
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            userUnitOfWord.Save();
            return Ok();




        }

        [HttpPut("UpdateStudent/{Id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateStudent( UpdateUserDto dTO , string Id)
        {
            var std = await userManager.FindByIdAsync(Id);
            if (std == null) return NotFound();



            if (dTO.Photo != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Photos/");
                string fullPath = Path.Combine(uploads, dTO.Photo.FileName);
                dTO.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));

                std.Photo = dTO.Photo.FileName;

            }

            std.Name = dTO.Name;
            std.PhoneNumber = dTO.PhoneNumber;
            std.Address = dTO.Address;
            std.Email = dTO.Email;
            std.Gender = dTO.Gender;
            std.UserName = dTO.UserName;

            var result = await userManager.UpdateAsync(std);

            if (result.Succeeded)
            {
                
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            userUnitOfWord.Save();
            return Ok("User Is Updated");

        }


        [HttpDelete("DeleteStudent/{Id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null) return NotFound(ModelState);

            await userManager.DeleteAsync(user);

            return Ok("The User Is Deleted");



        }





    }
}
