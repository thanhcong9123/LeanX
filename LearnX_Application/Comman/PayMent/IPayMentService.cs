using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.Base;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.PayMent;

namespace LearnX_Application.Comman.PayMent
{
    public interface IPayMentService : IEntityBaseRepository<Payment>
    {
        Task<PaymentCreatedResponse> CreateAsync(CreatePaymentRequest req);
        Task HandleMomoNotifyAsync(MomoNotifyDto dto);
        Task<Payment?> GetByOrderCodeAsync(string orderCode);
        Task HandleMomoReturnAsync(string orderId, int resultCode, string message, string? transId = null, string? signature = null);

    }
}