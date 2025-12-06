using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Application.ApiIntegration.Momo
{
    public class MomoCreatePaymentResponse
    {
        public string? PayUrl { get; set; }
        public string? RequestId { get; set; }
        public int ResultCode { get; set; }
        public string? Message { get; set; }
        public string? Signature { get; set; }
    }
}