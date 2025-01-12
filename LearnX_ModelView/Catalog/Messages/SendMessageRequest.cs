using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Messages
{
    public class SendMessageRequest
    {
        public Guid SenderId { get; set; }    // Người gửi
        public string ReceiverId { get; set; }  // Người nhận
        public string Content { get; set; }
    }
}