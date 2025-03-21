using Common.Handlers;
using Common.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgentService.Controllers
{
    [ApiController]
    [Route("api/agent")]
    public class AgentController : ControllerBase
    {
        private readonly GetAssignedChatsHandler _assignedChatsHandler;  

        public AgentController(GetAssignedChatsHandler assignedChatsHandler)
        {
            _assignedChatsHandler = assignedChatsHandler;
        }

        [HttpGet("assigned")]
        public async Task<IActionResult> GetAssignedChats()
        {
            var chats = await _assignedChatsHandler.Handle(new GetAssignedChatsQuery());
            return Ok(chats);
        }
    }
}
