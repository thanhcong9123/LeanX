using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Application.ApiIntegration.Momo;

namespace LearnX_Application.Comman.PayMent
{
    public interface IMomoClient
    {
         Task<MomoCreatePaymentResponse> CreatePaymentAsync(MomoCreatePaymentRequest request);
        bool VerifySignature(string rawData, string signature);
    }
}