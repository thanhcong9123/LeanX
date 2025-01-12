using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.EF;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Messages;
using Microsoft.EntityFrameworkCore;

namespace LearnX_Application.Comman
{
    public class MessageService : IMessageService

    {
        private readonly LearnXDbContext _context;

        public MessageService(LearnXDbContext context)
        {
            _context = context;
        }

        // Gửi tin nhắn
        public async Task<bool> SendMessageAsync(Guid senderId, string receiver, string content)
        {
            var receiverId = _context.Users
                                    .Where(u => u.Email == receiver)
                                    .Select(u => u.Id)
                                    .FirstOrDefault();
            if (receiverId == Guid.Empty || receiverId == senderId)
            {
                return false;
            }
            else
            {
                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    SentAt = DateTime.UtcNow,
                    IsRead = false
                };

                _context.Message.Add(message);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
        }

        // Lấy tin nhắn của người dùng
        public async Task<List<ViewMessage>> GetMessagesByUserAsync(Guid userId)
        {
            return await _context.Message
                                 .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                                 .OrderByDescending(m => m.SentAt)
                                 .Select(m => new ViewMessage
                                 {
                                     Id = m.Id,
                                     SenderId = m.SenderId,
                                     ReceiverId = m.ReceiverId,
                                     Content = m.Content,
                                     SentAt = m.SentAt,
                                     IsRead = m.IsRead,
                                     SenderEmail = _context.Users
                                                           .Where(u => u.Id == m.SenderId)
                                                           .Select(u => u.Email)
                                                           .FirstOrDefault(),
                                     ReceiverEmail = _context.Users
                                                             .Where(u => u.Id == m.ReceiverId)
                                                             .Select(u => u.Email)
                                                             .FirstOrDefault()
                                 })
                                 .ToListAsync();
        }


        // Đánh dấu tin nhắn đã đọc
        public async Task<bool> MarkMessageAsReadAsync(int messageId)
        {
            var message = await _context.Message.FindAsync(messageId);
            if (message != null && !message.IsRead)
            {
                message.IsRead = true;
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            return false;
        }

    }
}