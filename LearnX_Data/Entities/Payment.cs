using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Mã đơn để tra cứu/redirect (unique, dễ đọc)
        [Required, MaxLength(64)]
        public string OrderCode { get; set; } = default!;

        // Tham chiếu business
        public Guid UserId { get; set; }
        [MaxLength(64)]
        public string PackageCode { get; set; } = "premium_month";

        // Nhà cung cấp
        [MaxLength(32)]
        public string Provider { get; set; } = "MOMO";
        [MaxLength(128)]
        public string? ProviderOrderId { get; set; }
        [MaxLength(128)]
        public string? ProviderTransactionId { get; set; }

        // Số tiền
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [MaxLength(8)]
        public string Currency { get; set; } = "VND";

        // Trạng thái
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        [MaxLength(512)]
        public string? FailureReason { get; set; }

        // URLs và thời hạn
        [MaxLength(256)]
        public string? ReturnUrl { get; set; }
        [MaxLength(256)]
        public string? NotifyUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

        // Debug/trace
        public string? Metadata { get; set; }          // JSON phụ
        public string? RawRequest { get; set; }        // JSON gửi provider
        public string? RawResponse { get; set; }       // JSON nhận provider

        // Idempotency
        [MaxLength(100)]
        public string? IdempotencyKey { get; set; }

        [Timestamp]
        public byte[]? RowVersion { get; set; }

    }
    public enum PaymentStatus
    {
        Pending = 0,
        Processing = 1,
        Succeeded = 2,
        Failed = 3,
        Canceled = 4
    }
}