using chat.Server.models;
using chat.Server.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

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
        [HttpPost("chat/messages")]
        public async Task<IActionResult> GetMessagesFromChat([FromBody] SearchResult searchResult)
        {
            if (searchResult==null || searchResult.Users.Count == 0)
            {
                return NotFound();
            }

            var messages = _chatService.GetChatMessages(searchResult);
            if(messages == null)
            {
                return NotFound();
            }

            return Ok(messages);
        }
        [HttpGet("chat")]
        [ActionName("getProposedUsersByUserName")]
        public async Task<IActionResult> GetProposedChatConversationsBySearchInput([FromQuery] string searchPhrase)
        {
            var result = await _chatService.GetProposedChatConversationsBySearchInput(searchPhrase);
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
