using Common.Command;
using Common.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChatAPI.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly CreateChatSessionHandler _createChatHandler;
        private readonly GetActiveChatsHandler _getChatsHandler;
        private readonly CloseChatSessionHandler _closeChatHandler;

        public ChatController(CreateChatSessionHandler createChatHandler, 
                             GetActiveChatsHandler getChatsHandler, 
                             CloseChatSessionHandler closeChatHandler)
        {
            _createChatHandler = createChatHandler;
            _getChatsHandler = getChatsHandler;
            _closeChatHandler = closeChatHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateChat([FromBody] CreateChatSessionCommand command)
        {
            var chatId = await _createChatHandler.Handle(command);
            return Ok(new { ChatId = chatId });
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveChats()
        {
            var chats = await _getChatsHandler.Handle(new GetActiveChatsQuery());
            return Ok(chats);
        }

        [HttpPut("close")]
        public async Task<IActionResult> CloseChat([FromBody] CloseChatSessionCommand command)
        {
            var result = await _closeChatHandler.Handle(command);
            return Ok(new { Message = result });
        }
    }
}
