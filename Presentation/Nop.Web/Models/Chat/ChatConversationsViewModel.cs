using System.Collections.Generic;

namespace Nop.Web.Models.Chat
{
    public class ChatConversationsViewModel
    {
        public List<ChatConversationsModel> ChatUsersModels { get; set; }
        public ChatUsersModel ChatUsersModel { get; set; }
    }
}
