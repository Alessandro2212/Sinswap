using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Chats;
using System.Collections.Generic;

namespace Nop.Services.Chats
{
    public interface IChatService
    {
        List<Chat> GetLatestChatsByUser(int userId, string status);
        List<Chat> GetChatsOfUser(int userId, int partnerId);
        Chat SaveChatMessage(int userId, int partnerId, string message, IFormFile formFile);
        void DeleteChatMessage(int userId, int partnerId);
        void ResumeChatMessage(int userId, int partnerId);
        void UpdateChats(IEnumerable<Chat> chats);
        int GetChatMessageStatusId(string chatStatus);
    }
}
