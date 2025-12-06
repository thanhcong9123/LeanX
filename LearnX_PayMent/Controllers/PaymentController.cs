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
                await _payMentService.HandleMomoNotifyAsync(notify);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("ReceiveMomoResponse")]
        public async Task<IActionResult> ReceiveMomoResponse(
            [FromQuery] string orderId,
            [FromQuery] int resultCode,
            [FromQuery] string message)
        {
            var payment = await _payMentService.GetByOrderCodeAsync(orderId);

            if (payment == null)
            {
                return NotFound("Payment not found");
            }

            // Redirect to frontend with payment result
            var redirectUrl = $"{payment.ReturnUrl}?orderCode={orderId}&status={resultCode}&message={Uri.EscapeDataString(message)}";
            return Redirect(redirectUrl);
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
