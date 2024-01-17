using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Chats;
using System.Collections.Generic;

namespace Nop.Services.Chats
{
    public interface IChatService
    {
        List<Chat> GetLatestChatsByUser(int userId);
        List<Chat> GetChatsOfUser(int userId, int partnerId);
        Chat SaveChatMessage(int userId, int partnerId, string message, IFormFile formFile);
        void DeleteChatMessage(int userId, int partnerId);
        void UpdateChatsAsRead(IEnumerable<Chat> chats);
    }
}
