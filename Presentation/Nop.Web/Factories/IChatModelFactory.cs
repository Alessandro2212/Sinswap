using Nop.Web.Models.Chat;

namespace Nop.Web.Factories
{
    public interface IChatModelFactory
    {
        ChatUsersViewModel GetChatUsersViewModel(int userId, string chatStatus);
        ChatConversationsViewModel GetChatConversationsViewModel(int userId, int partnerId);
    }
}
