using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.PayMent
{
    public class CreatePaymentRequest
    {
        public Guid UserId { get; set; }
        public string PackageCode { get; set; } = "premium_month";
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string ReturnUrl { get; set; } = "";
        public string NotifyUrl { get; set; } = "";
        public string? IdempotencyKey { get; set; }
    }
    public class PaymentCreatedResponse
    {
        public string OrderCode { get; set; } = default!;
        public string PayUrl { get; set; } = default!;
        public DateTime? ExpiresAt { get; set; }
    }
    public class PaymentStatusResponse
    {
        public string? OrderId { get; set; }
        public string? Status { get; set; } // Pending, Success, Failed
        public decimal? Amount { get; set; }
        public string? Raw { get; set; }
    }
     public class UpgradeRequestModel
    {
        [Required]
        public decimal Amount { get; set; }

        public string? PackageName { get; set; }

        public string? ExtraData { get; set; }
    }
    public class UpgradePaymentViewModel
    {
        public string OrderId { get; set; } = "";
        public string PayUrl { get; set; } = "";
        public decimal Amount { get; set; }
        public string PackageName { get; set; } = "";
    }
}