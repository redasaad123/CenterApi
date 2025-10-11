using Core.Interfaces;
using Core.Model;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using WebApi.DTO.AuthenticateDTO;


namespace WebApi.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting;
        private readonly IUnitOfWork<TypeJop> typeUnitOfWork;
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly PasswordHasher<AppUser> passwordHasher;

        public AuthenticateController(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hosting , IUnitOfWork<TypeJop> typeUnitOfWork, IUnitOfWork<AppUser> UserUnitOfWork, PasswordHasher<AppUser> passwordHasher)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.hosting = hosting;
            this.typeUnitOfWork = typeUnitOfWork;
            userUnitOfWork = UserUnitOfWork;
            this.passwordHasher = passwordHasher;
            this.hosting = hosting;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO dto)
        {
            var errors = ModelState.Values.SelectMany(r => r.Errors);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool nullPhoto = true; 

            if (dto.Photo != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, @"Photos/");
                string fullPath = Path.Combine(uploads, dto.Photo.FileName);
                dto.Photo.CopyTo(new FileStream(fullPath, FileMode.Create));
                nullPhoto = false;

            }

            var validuser = await userManager.FindByEmailAsync(dto.Email);

            if (validuser != null)
            {
                ModelState.AddModelError("Email", "Email Is already exists");
                return BadRequest(ModelState);


            }

            var sutdId = typeUnitOfWork.Entity.Find(x => x.JopType.Contains("student"));

            var user = new AppUser
            {
                Name = dto.Name,
                Address = dto.Address,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberParent = dto.PhoneNumberParent,
                Photo = nullPhoto ? "Photos/149071.png" : dto.Photo.FileName ,
                CreatedAt = DateTime.Now,
                TypeJopId = sutdId.Id,
                UserName = new MailAddress(dto.Email).User

            };

            var result = await userManager.CreateAsync(user, dto.Password+ "Abcd123#");

            if (result.Succeeded)
            {
                
                var Claim = new Claim("User", "User");
                await userManager.AddClaimAsync(user, Claim);

                var roleIsExists = await roleManager.RoleExistsAsync("Teacher");
                if (roleIsExists)
                {
                    await userManager.AddToRoleAsync(user, "Student");
                }
                else
                {
                    await roleManager.CreateAsync(new IdentityRole("Student"));
                    await userManager.AddToRoleAsync(user, "Student");
                }
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }
            typeUnitOfWork.Save();
            return Ok();
        }


        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDTO dTO)
        {
            var errors = ModelState.Values.SelectMany(r => r.Errors);
            if (ModelState.IsValid)
            {
                var username = new EmailAddressAttribute().IsValid(dTO.Email) ? new MailAddress(dTO.Email).User : dTO.Email;

                var user = await userManager.FindByNameAsync(username);

                if (user != null)
                {
                    var Valid = await userManager.CheckPasswordAsync(user, dTO.Password+ "Abcd123#");
                    if (Valid)
                    {
                        var claims = new List<Claim>();
                        //Token Genreted Id Chenge ( jwt predefied claims)

                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                        var userRole = await userManager.GetRolesAsync(user);

                        foreach (var role in userRole)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:securitKey"]));

                        var signing = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);


                        //design token
                        var token = new JwtSecurityToken(

                            issuer: configuration["jwt:Issuer"],
                            audience: configuration["jwt:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            claims: claims,
                            signingCredentials: signing


                        );

                        //generate token response


                        return Ok(
                            new
                            {
                                token = new JwtSecurityTokenHandler().WriteToken(token),
                                expiration = DateTime.Now.AddHours(1),

                            }




                        );

                    }

                }
                ModelState.AddModelError("UserName", "UserName Or Password InValid");

            }
            return BadRequest(ModelState);


        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO DTO)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var UserName = new MailAddress(DTO.Email).User;

            
            var user = await userManager.FindByNameAsync(UserName);

            if (user == null)
                return NotFound("User Is NotFound");

            return Ok(user.Id);

        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(BasePasswordDTO DTO , string Id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await userManager.FindByIdAsync(Id);
            if (user == null)
                return NotFound("User Is NotFound");

            var Hashpassword = passwordHasher.HashPassword(user, DTO.NewPassword+ "Abcd123#");

            user.PasswordHash = Hashpassword;
            userManager.UpdateAsync(user);
            typeUnitOfWork.Save();

            return Ok("The Password Is Changed");

        }








    }
}
