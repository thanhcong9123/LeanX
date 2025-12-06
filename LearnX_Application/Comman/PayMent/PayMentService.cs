using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LearnX_Application.ApiIntegration.Momo;
using LearnX_Application.Base;
using LearnX_Data.Entities;
using LearnX_Data.Entities.EF;
using LearnX_ModelView.Catalog.PayMent;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman.PayMent
{
    public class PayMentService : EntityBaseRepository<Payment>, IPayMentService
    {
        private readonly LearnXPayMentDbContext _contextData;
        private readonly IMomoClient _momoClient;
        public PayMentService(LearnXPayMentDbContext context, IMomoClient momoClient) : base(context)
        {
            _contextData = context;
            _momoClient = momoClient;
        }
        public async Task<PaymentCreatedResponse> CreateAsync(CreatePaymentRequest req)
        {
            var orderCode = $"ORD_{DateTime.UtcNow:yyyyMMddHHmmss}_{Guid.NewGuid().ToString("N")[..8]}";
            // Create payment entity
            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderCode = orderCode,
                UserId = req.UserId,
                PackageCode = req.PackageCode,
                Provider = "MOMO",
                Amount = req.Amount,
                Currency = req.Currency,
                Status = PaymentStatus.Pending,
                ReturnUrl = req.ReturnUrl,
                NotifyUrl = req.NotifyUrl,
                IdempotencyKey = req.IdempotencyKey,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
            //call api Momo
            var momoRequest = new MomoCreatePaymentRequest
            {
                OrderCode = orderCode,
                Amount = req.Amount,
                OrderInfo = $"Payment for package {req.PackageCode}",
                ReturnUrl = req.ReturnUrl,
                NotifyUrl = req.NotifyUrl,
                ExtraData = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes
                (
                    JsonSerializer.Serialize(new { userId = req.UserId, packageCode = req.PackageCode }
                )))
            };
            var momoResponse = await _momoClient.CreatePaymentAsync(momoRequest);
            payment.RawRequest = JsonSerializer.Serialize(momoRequest);
            payment.RawResponse = JsonSerializer.Serialize(momoResponse);
            if (momoResponse.ResultCode == 0 && !string.IsNullOrEmpty(momoResponse.PayUrl))
            {
                payment.Status = PaymentStatus.Processing;
                payment.ProviderOrderId = momoResponse.RequestId;

            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = momoResponse.Message;
            }
            await _contextData.Payments.AddAsync(payment);
            await _contextData.SaveChangesAsync();
            return new PaymentCreatedResponse
            {
                OrderCode = orderCode,
                PayUrl = momoResponse.PayUrl ?? "",
                ExpiresAt = payment.ExpiresAt
            };

        }
        public async Task HandleMomoNotifyAsync(MomoNotifyDto dto)
        {
            var payment = await _contextData.Payments
                .FirstOrDefaultAsync(p => p.OrderCode == dto.orderId);
            if (payment == null)
            {
                throw new Exception($"Payment with OrderCode {dto.orderId} not found.");
            }
            var rawHash = $"accessKey={dto.requestId}" +
                        $"&amount={dto.amount}" +
                        $"&extraData={dto.extraData}" +
                        $"&message={dto.message}" +
                        $"&orderId={dto.orderId}" +
                        $"&orderInfo={dto.orderInfo}" +
                        $"&orderType={dto.orderType}" +
                        $"&partnerCode=MOMO" +
                        $"&payType={dto.payType}" +
                        $"&requestId={dto.requestId}" +
                        $"&responseTime={dto.responseTime}" +
                        $"&resultCode={dto.resultCode}" +
                        $"&transId={dto.transId}";

            if (!_momoClient.VerifySignature(rawHash, dto.signature))
            {
                throw new InvalidOperationException("Invalid signature from MoMo");
            }
            payment.ProviderTransactionId = dto.transId.ToString();
            payment.UpdatedAt = DateTime.UtcNow;
            payment.RawResponse = JsonSerializer.Serialize(dto);

            if (dto.resultCode == 0)
            {
                payment.Status = PaymentStatus.Succeeded;
                payment.PaidAt = DateTime.UtcNow;

                // TODO: Publish payment.succeeded event here for user premium upgrade
                // await _eventPublisher.PublishAsync(new PaymentSucceededEvent { ... });
            }
            else
            {
                payment.Status = PaymentStatus.Failed;
                payment.FailureReason = dto.message;
            }

            _contextData.Payments.Update(payment);
            await _contextData.SaveChangesAsync();
        }
        public async Task<Payment?> GetByOrderCodeAsync(string orderCode)
        {
             return await _contextData.Payments
                .FirstOrDefaultAsync(p => p.OrderCode == orderCode);
        }
    }
}