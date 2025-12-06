using LearnX_Application.Comman.PayMent;
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
        [HttpGet]
        public IActionResult GetPayments()
        {
            
            // Implementation for retrieving payments
            return Ok();
        }
    }
}
