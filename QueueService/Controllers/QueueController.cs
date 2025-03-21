using Common.Handlers;
using Common.Queries;
using Microsoft.AspNetCore.Mvc;
using QueueService.Service.Interface;
using System.Threading.Tasks;

namespace QueueService.Controllers
{
    [ApiController]
    [Route("api/queue")]
    public class QueueController : ControllerBase
    {
        private readonly IQueueService _queueService;
        private readonly GetPendingChatsHandler _pendingChatsHandler;

        public QueueController(IQueueService queueService, GetPendingChatsHandler pendingChatsHandler)
        {
            _queueService = queueService;
            _pendingChatsHandler = pendingChatsHandler;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingChats()
        {
            var chats = await _pendingChatsHandler.Handle(new GetPendingChatsQuery());
            return Ok(chats);
        }
    }
}