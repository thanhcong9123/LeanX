using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Application.ApiIntegration.Momo
{
    public class MomoOptions
    {
         public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string MomoApiUrl { get; set; } = default!;
        public string ReturnUrl { get; set; } = default!;
        public string NotifyUrl { get; set; } = default!;
        public string PartnerCode { get; set; } = default!;
        public string RequestType { get; set; } = default!;
    }
}