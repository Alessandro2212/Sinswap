using System;

namespace Nop.Web.Models.Chat
{
    public class ChatConversationsModel
    {       
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Message { get; set; }
        public string MessagePictureUrl { get; set; }
        public DateTime Time { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}