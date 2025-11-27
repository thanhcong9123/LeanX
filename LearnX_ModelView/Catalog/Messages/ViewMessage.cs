using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnX_ModelView.Catalog.Messages
{
    public class ViewMessage
    {
        public int Id { get; set; }               
        public Guid SenderId { get; set; }      
        public Guid ReceiverId { get; set; }     
        public string Content { get; set; }      
        public DateTime SentAt { get; set; }   
        public bool IsRead { get; set; }

        // Thuộc tính không ánh xạ
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
    }
}