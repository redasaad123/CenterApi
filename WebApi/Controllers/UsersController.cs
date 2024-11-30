using Core.Consts;
using Core.Interfaces;
using Core.Model;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using WebApi.DTO.AuthenticateDTO;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApi.DTO.UserDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("AdminRole")]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<TypeJop> TypeJopUnitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;

        

        public UsersController(RoleManager<IdentityRole> roleManager, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting, IUnitOfWork<AppUser> UserUnitOfWork , UserManager<AppUser> userManager, IUnitOfWork<TypeJop> TypeJopUnitOfWork)
        {
            userUnitOfWork = UserUnitOfWork;
            this.userManager = userManager;
            this.TypeJopUnitOfWork = TypeJopUnitOfWork;
            this.roleManager = roleManager;
            this.hosting = hosting;
        }


        [HttpGet("{typeJop}")]
        public async Task<IActionResult> GetUsers(string typeJop)
        {

            var typeid = TypeJopUnitOfWork.Entity.Find(x=>x.JopType.Contains(typeJop));

            var users = await userUnitOfWork.Entity.FindAll(x=>x.TypeJopId == typeid.Id , user => new GetUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Email = user.Email,
                UserName = user.UserName,
                Gender = user.Gender,
                PhotoUrl = user.Photo,
                PhoneNumberParent = user.PhoneNumberParent,
                joptype = typeid.JopType,

                
            });
            if (users == null)
                return NotFound("Not Found Users");

            

            return Ok(users);

        }

        [HttpGet("UserById/{UserId}")]
        public async Task<IActionResult> UserById(string UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
                return NotFound("This User Not Found");


            var model = new GetUserDTO
            {
                Id = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                Email = user.Email,
                UserName = user.UserName,
                Gender = user.Gender,
            };


            return Ok (model);

        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromForm] AddUserDTO dto , string TypeJop)
        {
            var errors = ModelState.Values.SelectMany(r => r.Errors);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if(dto.TypeJop != "Student")
            //    return BadRequest("Non-students are not allowed to register");

            var validuserbyNationa = await userManager.FindByNameAsync(dto.NationaNumber);

            if (validuserbyNationa != null)
            {
                ModelState.AddModelError("NationaNumber", "NationalNumber Is already exists");
                return BadRequest(ModelState);


            }

            var sutdId = TypeJopUnitOfWork.Entity.Find(x => x.JopType.Contains(TypeJop));

            var user = new AppUser
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                TypeJopId = sutdId.Id,
                UserName = dto.NationaNumber,
                CreatedAt = DateTime.Now,
                Photo = "Photos/149071.png",

            }; 

            var result = await userManager.CreateAsync(user, dto.NationaNumber+ "Abcd123#");

            if (result.Succeeded)
            {
                var Claim = new Claim("User", "User");
                await userManager.AddClaimAsync(user, Claim);

                var roleIsExists = await roleManager.RoleExistsAsync(sutdId.JopType);
                if (roleIsExists)
                {
                    await userManager.AddToRoleAsync(user, sutdId.JopType);
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole(sutdId.JopType));
                    await userManager.AddToRoleAsync(user, sutdId.JopType);
                }

            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            userUnitOfWork.Save();
            return Ok();


        }

        [HttpPut("EditUser/{Id}")]
        public async Task<IActionResult> EditUser([FromForm] UpdateUserDto userDTO,  string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound(ModelState);

            if (userDTO.Photo != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Photos/");
                string fullPath = Path.Combine(uploads, userDTO.Photo.FileName);
                userDTO.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));

                user.Photo = userDTO.Photo.FileName;

            }

            user.Name = userDTO.Name;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.Address = userDTO.Address;
            user.Email = userDTO.Email;
            user.Gender = userDTO.Gender;
            user.UserName = userDTO.UserName;

            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            TypeJopUnitOfWork.Save();
            return Ok("User Is Updated");


        }

        [HttpDelete("DeleteUser")]
        public async Task< IActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null) return NotFound(ModelState);

            await userManager.DeleteAsync(user);

            return Ok("The User Is Deleted");



        }





        



         



    }
}
