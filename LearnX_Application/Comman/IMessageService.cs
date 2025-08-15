using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Messages;

namespace LearnX_Application.Comman
{
    public interface IMessageService
    {
        Task<bool> SendMessageAsync(Guid senderId, string receiverId, string content);
        Task<List<ViewMessage>> GetMessagesByUserAsync(Guid userId);
        Task<bool> MarkMessageAsReadAsync(int messageId);
        Task<Guid?> GetUserIdByEmailAsync(string email);
    }
}