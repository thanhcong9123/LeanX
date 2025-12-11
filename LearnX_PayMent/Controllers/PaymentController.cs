using LearnX_Application.Comman.PayMent;
using LearnX_ModelView.Catalog.PayMent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayMentService _payMentService;
        public PaymentController(IPayMentService payMentService)
        {
            _payMentService = payMentService;
        }
        
        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentRequest request)
        {
            try
            {
                var response = await _payMentService.CreateAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("MomoNotify")]
        public async Task<IActionResult> MomoNotify([FromBody] MomoNotifyDto notify)
        {
            try
            {
                Console.WriteLine("Received Momo Notify: " + System.Text.Json.JsonSerializer.Serialize(notify));
                await _payMentService.HandleMomoNotifyAsync(notify);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MomoNotify: {ex.Message}");
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ReceiveMomoResponse")]
        public async Task<IActionResult> ReceiveMomoResponse(
            [FromQuery] string orderId,
            [FromQuery] int resultCode,
            [FromQuery] string message,
            [FromQuery] string? transId = null,
            [FromQuery] string? signature = null)
        {
            try
            {
                Console.WriteLine($"Received Momo Response: orderId={orderId}, resultCode={resultCode}, message={message}");
                
                // Xử lý payment trước
                await _payMentService.HandleMomoReturnAsync(orderId, resultCode, message, transId, signature);
                
                // Lấy payment để redirect về frontend
                var payment = await _payMentService.GetByOrderCodeAsync(orderId);
                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                // Tạo URL redirect về frontend với các thông tin cần thiết
                var status = resultCode == 0 ? "success" : "failed";
                var redirectUrl = $"{payment.ReturnUrl}?orderCode={orderId}&status={status}&message={Uri.EscapeDataString(message)}";
                
                return Redirect(redirectUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ReceiveMomoResponse: {ex.Message}");
                // Nếu có lỗi, vẫn redirect về frontend với status error
                return Redirect($"http://localhost:5184/payment/return?status=error&message={Uri.EscapeDataString(ex.Message)}");
            }
        }

        [HttpGet("{orderCode}")]
        public async Task<IActionResult> GetPayment(string orderCode)
        {
            var payment = await _payMentService.GetByOrderCodeAsync(orderCode);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }
    }
}
