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
        public async Task<IActionResult> CreateUpgrade(CreatePaymentRequest model)
        {
            if (!ModelState.IsValid) return View("Upgrade", model);

            var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(uid, out var userId))
            {
                return Forbid();
            }

            // Build request to PaymentService
            var paymentReq = new CreatePaymentRequest
            {
                UserId = userId,
                PackageCode = model.PackageCode,
                Amount = model.Amount,
                Currency = "VND",
                ReturnUrl = $"{Request.Scheme}://{Request.Host}/payment/return",
                NotifyUrl = $"{Request.Scheme}://{Request.Host}/api/payment/MomoNotify",
                IdempotencyKey = model.IdempotencyKey ?? Guid.NewGuid().ToString()
            };

            var resp = await _paymentClient.CreatePaymentAsync(paymentReq);
            if (resp == null)
            {
                TempData["Error"] = "Không thể tạo giao dịch thanh toán. Vui lòng thử lại.";
                return RedirectToAction("Upgrade");
            }
            // Pass payUrl & orderId to view that will render QR
            return View("UpgradeQr", new UpgradePaymentViewModel
            {
                OrderId = resp.OrderCode,
                PayUrl = resp.PayUrl,
                Amount = model.Amount,
                PackageName = model.PackageCode
            });

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