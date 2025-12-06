using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Application.ApiIntegration.Momo
{
    public class MomoCreatePaymentRequest
    {
         public string OrderCode { get; set; } = default!;
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; } = default!;
        public string ReturnUrl { get; set; } = default!;
        public string NotifyUrl { get; set; } = default!;
        public string ExtraData { get; set; } = "";
    }
}