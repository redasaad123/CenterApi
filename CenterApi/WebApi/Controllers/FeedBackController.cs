using Core.Interfaces;
using Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTO.FeedBackDTO;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class FeedBackController : ControllerBase
    {
        private readonly IUnitOfWork<AppUser> userUnitOfWork;
        private readonly IUnitOfWork<feedBack> feedBackUnitOfWork;
        private readonly IUnitOfWork<Materails> materailsUnitOfWork;
        private readonly UserManager<AppUser> userManager;

        public FeedBackController(IUnitOfWork<AppUser> UserUnitOfWork, IUnitOfWork<feedBack> feedBackUnitOfWork, IUnitOfWork<Materails> MaterailsUnitOfWork, UserManager<AppUser> userManager)
        {
            userUnitOfWork = UserUnitOfWork;
            this.feedBackUnitOfWork = feedBackUnitOfWork;
            materailsUnitOfWork = MaterailsUnitOfWork;
            this.userManager = userManager;
        }

        [HttpGet("GetFeedBacks/{materailId}")]
        public async Task<IActionResult> GetFeedBacks(string materailId)
        {

            var material = await materailsUnitOfWork.Entity.GetAsync(materailId);

            var feedback = await feedBackUnitOfWork.Entity.FindAll(x => x.MaterailId == materailId, m => new GetFeedBackDTO
            {

                FeedBackId = m.MessageId,
                message = m.Message,
                studentName = userUnitOfWork.Entity.GetAsync(m.StudentId).Result.Name,

            });
            if (feedback.Count() == 0)
                return NotFound("No FeedBack From Student");

            return Ok(feedback);


        }

        [HttpGet("GetFeedBackId/{feedBackId}")]
        public async Task<IActionResult> GetFeedBackId(string feedBackId)
        {
            var feedback = await feedBackUnitOfWork.Entity.GetAsync(feedBackId);
            if (feedback == null)
                return NotFound();

            var materail = await materailsUnitOfWork.Entity.GetAsync(feedback.MaterailId);
            var std = await userUnitOfWork.Entity.GetAsync(feedback.StudentId);


            var model = new GetFeedBackDTO
            {
                FeedBackId = feedback.MessageId,
                message = feedback.Message,
                materailName = materail.materailName,
                studentName = std.Name,
            };

            return Ok(model);


        }

        [HttpPost]
        public async Task<IActionResult> AddFeedBack(string materailId, [FromForm] AddMessageDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = userManager.GetUserId(HttpContext.User);

            var message = new feedBack
            {
                MessageId = Guid.NewGuid().ToString(),
                MaterailId = materailId,
                StudentId = user,
                Message = dto.message,
            };

            await feedBackUnitOfWork.Entity.AddAsync(message);
            feedBackUnitOfWork.Save();
            return Ok();
        }


        [HttpPut("{MessageId}")]
        public async Task<IActionResult> UpdateMessage(string MessageId, [FromForm] UpdateMessageDTO dto)
        {
            var message = await feedBackUnitOfWork.Entity.GetAsync(MessageId);
            if (message == null)
                return NotFound(ModelState);
            message.Message = dto.message;
            await feedBackUnitOfWork.Entity.UpdateAsync(message);
            feedBackUnitOfWork.Save();
            return Ok();
        }

        [HttpDelete("{MessageId}")]
        public async Task< IActionResult> DeleteMessage(string MessageId)
        {
            var message = await feedBackUnitOfWork.Entity.GetAsync(MessageId);
            if (message == null)
                return NotFound(ModelState);
             feedBackUnitOfWork.Entity.Delete(message);
            feedBackUnitOfWork.Save();
            return Ok();

        }

    }
}
