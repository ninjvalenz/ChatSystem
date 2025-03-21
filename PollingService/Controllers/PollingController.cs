using Common.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PollingService.Controllers
{
    [ApiController]
    [Route("api/polling")]
    public class PollingController : ControllerBase
    {
        private readonly GetActiveChatsHandler _activeChatsHandler;  

        public PollingController(GetActiveChatsHandler activeChatsHandler)
        {
            _activeChatsHandler = activeChatsHandler;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveChats()
        {
            var chats = await _activeChatsHandler.Handle(new GetActiveChatsQuery());
            return Ok(chats);
        }
    }
}
