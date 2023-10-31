using Nop.Core.Domain.Chats;
using System.Collections.Generic;

namespace Nop.Services.Chats
{
    public interface IChatService
    {
        List<Chat> GetLatestChatsByUser(int userId);
        List<Chat> GetChatsOfUser(int userId, int partnerId);
        Chat SaveChatMessage(int userId, int partnerId, string message);
    }
}
