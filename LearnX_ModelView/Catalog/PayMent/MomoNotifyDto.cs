using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.PayMent
{
    public class MomoNotifyDto
    {
        public string orderId { get; set; } = default!;
        public string requestId { get; set; } = default!;
        public long amount { get; set; }
        public string orderInfo { get; set; } = default!;
        public string orderType { get; set; } = default!;
        public long transId { get; set; }
        public int resultCode { get; set; }           // 0 = success
        public string message { get; set; } = default!;
        public string signature { get; set; } = default!;
        public string extraData { get; set; } = "";
        public long responseTime { get; set; }
        public string payType { get; set; } = "";
    }
}