using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_ModelView.Catalog.PayMent;

namespace LearnX_ApiIntegration.PayMent
{
    public interface IPaymentApiClient
    {
        Task<PaymentCreatedResponse?> CreatePaymentAsync(CreatePaymentRequest request);
        Task<PaymentStatusResponse?> GetPaymentStatusAsync(string orderId);
    }
}