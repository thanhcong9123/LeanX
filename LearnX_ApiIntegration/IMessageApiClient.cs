using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnX_Data.Entities;
using LearnX_ModelView.Catalog.Messages;
using LearnX_ModelView.Common;

namespace LearnX_ApiIntegration
{
    public interface IMessageApiClient
    {
        Task<bool> SendMessageAsync(SendMessageRequest request);
        Task<List<ViewMessage>> GetMessagesAsync(Guid userId);
        Task<ApiResult<string>> MarkMessageAsReadAsync(int messageId);
        Task<Guid?> GetUserIdByEmailAsync(string email);
    }
}