using chat.Server.models;
using chat.Server.services;
using Microsoft.AspNetCore.Mvc;

namespace chat.Server.Controllers
{
    [Route("/api")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            this._chatService = chatService;
        }
        //[HttpGet("chat/{userName}")]
        //public async Task<IActionResult> GetChatOnUserNameClick([FromQuery] string userName)
        //{
        //    var result = await _chatService.GetChat(userName);
        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(result);
        //}

        [HttpGet("chat")]
        [ActionName("getProposedUsersByUserName")]
        public async Task<IActionResult> GetProposedUsersByUserName([FromQuery] string userName)
        {
            var result = await _chatService.GetSimilarUsers(userName);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }



        [HttpPost("chat/message")]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            bool result = await _chatService.AddMessage(message);
            if (result) return Created();

            return BadRequest();
        }

    } 
}
