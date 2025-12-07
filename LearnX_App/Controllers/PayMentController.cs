using System;
using System.Security.Claims;
using System.Threading.Tasks;
using LearnX_ApiIntegration;
using LearnX_ApiIntegration.PayMent;
using LearnX_App.Models;
using LearnX_ModelView.Catalog.PayMent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LearnX_App.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentApiClient _paymentClient;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(IPaymentApiClient paymentClient, ILogger<PaymentController> logger)
        {
            _paymentClient = paymentClient;
            _logger = logger;
        }

        // Show Upgrade page with a button
        [HttpGet]
        public IActionResult Upgrade()
        {
            return View();
        }

        // Called when user clicks "Start payment" (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUpgrade(UpgradeRequestModel model)
        {
            if (!ModelState.IsValid) return View("Upgrade", model);

            // Get current user id from claims (assuming NameIdentifier contains GUID)
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdStr, out var userId))
            {
                _logger.LogWarning("Cannot find user id in claims");
                return Forbid();
            }

            var req = new CreatePaymentRequest
            {
                UserId = userId,
                Amount = model.Amount,
                PackageCode = model.PackageName ?? "Premium 1 tháng",
            };

            var resp = await _paymentClient.CreatePaymentAsync(req);
            if (resp == null)
            {
                ModelState.AddModelError("", "Không thể tạo giao dịch thanh toán. Vui lòng thử lại.");
                return View("Upgrade", model);
            }

            if (string.IsNullOrEmpty(resp.PayUrl) || string.IsNullOrEmpty(resp.OrderCode))
            {
                ModelState.AddModelError("", $"Thanh toán thất bại: {resp}");
                return View("Upgrade", model);
            }

            var vm = new UpgradePaymentViewModel
            {
                OrderId = resp.OrderCode,
                PayUrl = resp.PayUrl
            };

            return View("UpgradeQr", vm);
        }

        // AJAX poll endpoint for client to ask status
        [HttpGet]
        public async Task<IActionResult> CheckStatus(string orderId)
        {
            if (string.IsNullOrEmpty(orderId)) return BadRequest();

            var status = await _paymentClient.GetPaymentStatusAsync(orderId);
            if (status == null) return StatusCode(500);
            return Ok(status);
        }
    }
}