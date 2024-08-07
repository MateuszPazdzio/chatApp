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
        [HttpGet("chat/{userName}")]
        public async Task<IActionResult> GetChatOnUserNameClick([FromQuery] string userName)
        {
            var result = await _chatService.GetChat(userName);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
