using LearnX_Application.Comman;
using LearnX_ModelView.Catalog.Messages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // Gửi tin nhắn
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendMessage([FromForm]SendMessageRequest request)
        {
            if (request.SenderId == Guid.Empty || string.IsNullOrEmpty(request.ReceiverId)|| string.IsNullOrEmpty(request.Content))
            {
                return BadRequest();
            }

            var result = await _messageService.SendMessageAsync(request.SenderId, request.ReceiverId, request.Content);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        // Lấy tất cả tin nhắn của người dùng
        [HttpGet]
        public async Task<IActionResult> GetMessages(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var result = await _messageService.GetMessagesByUserAsync(userId);

            if (result != null && result.Any())
            {
                return Ok(result);
            }

            return NoContent();

        }

        // Đánh dấu tin nhắn là đã đọc
        [HttpPut("mark-as-read")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            if (messageId <= 0)
            {
                return BadRequest();
            }

            var result = await _messageService.MarkMessageAsReadAsync(messageId);

            if (result)
            {
                return Ok(result);
            }

            return NoContent();
        }

        // Lấy User ID từ email
        [HttpGet("get-user-id-by-email")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest();
            }

            var userId = await _messageService.GetUserIdByEmailAsync(email);

            if (userId.HasValue)
            {
                return Ok(userId.Value);
            }

            return NotFound();
        }
    }
}
