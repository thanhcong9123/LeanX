using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_Data.Entities
{
    public class Message
    {
        public int Id { get; set; }               // ID của tin nhắn
        public Guid SenderId { get; set; }        // ID của người gửi (lấy từ Identity User)
        public Guid ReceiverId { get; set; }      // ID của người nhận (lấy từ Identity User)
        public string Content { get; set; }       // Nội dung tin nhắn
        public DateTime SentAt { get; set; }      // Thời gian gửi tin nhắn
        public bool IsRead { get; set; }
    }
}