using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Messages
{
    public class ConversationSummary
    {
        public Guid UserId { get; set; }              // Id của người còn lại trong hội thoại (nếu bạn gửi thì là ReceiverId, nếu bạn nhận thì là SenderId)
    public string Email { get; set; }             // Email của người đó
    public string LatestMessage { get; set; }     // Nội dung tin nhắn mới nhất
    public DateTime LatestAt { get; set; }        // Thời điểm gửi mới nhất
    public int UnreadCount { get; set; }          // Số tin chưa đọc
    public bool LatestIsRead { get; set; } 
    }
}