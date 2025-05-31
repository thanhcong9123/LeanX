using System.Security.Claims;
using LearnX_ApiIntegration;
using LearnX_App.Hubs;
using LearnX_ModelView.Catalog.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MyApp.Namespace
{
    [Authorize]
    public class MessagesController : Controller
    {
        // GET: MessagesController
        private readonly IMessageApiClient _messageApiClient;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(IMessageApiClient messageApiClient, IHubContext<ChatHub> hubContext)
        {
            _messageApiClient = messageApiClient;
            _hubContext = hubContext;

        }

        // Trang gửi tin nhắn
        [HttpGet("send")]
        public IActionResult SendMessage()
        {
            return View();
        }

        // Gửi tin nhắn từ WebApp
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(SendMessageRequest model)
        {
            if (ModelState.IsValid)
            {
                var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                model.SenderId = userId;

                var success = await _messageApiClient.SendMessageAsync(model);
                if (success)
                {
                    await _hubContext.Clients.User(model.ReceiverId.ToString())
                        .SendAsync("ReceiveMessage", User.Identity.Name, model.Content);
                    ViewBag.SuccessMessage = "Message sent successfully.";
                    return RedirectToAction("GetMessages");
                }
                ViewBag.ErrorMessage = "Email error. Please try again.";
            }
            return View(model);
        }

        // Lấy danh sách tin nhắn
        [HttpGet("inbox")]
        public async Task<IActionResult> GetMessages()
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value); // Lấy userId từ Claims
            var messages = await _messageApiClient.GetMessagesAsync(userId);
            ViewBag.UserId = userId;
            return View(messages);
        }

        // Đánh dấu tin nhắn là đã đọc
        [HttpPost("mark-as-read/{messageId}")]
        public async Task<IActionResult> MarkMessageAsRead(int messageId)
        {
            var success = await _messageApiClient.MarkMessageAsReadAsync(messageId);
            if (success.IsSuccessed)
            {
                ViewBag.SuccessMessage = "Message marked as read.";
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to mark message as read.";
            }
            return RedirectToAction("GetMessages");
        }
        [HttpGet("chat")]
        public async Task<IActionResult> Chat()
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var messages = await _messageApiClient.GetMessagesAsync(userId);
            if (messages == null)
            {
                messages = new List<ViewMessage>();
            }
            ViewBag.UserId = userId;
            return View(messages);
        }

        [HttpPost("send-from-chat")]
        public async Task<IActionResult> SendFromChat([FromBody] SendMessageRequest model)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            model.SenderId = userId;

            var success = await _messageApiClient.SendMessageAsync(model);

            if (success)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", HttpContext.User.Identity.Name, model.Content, DateTime.Now.ToString("HH:mm:ss"));
                return Json(new { success = true });
            }

            return BadRequest(new { success = false, message = "Failed to send" });
        }



    }
}