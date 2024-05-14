using System;

namespace Nop.Web.Models.Chat
{
    public class ChatUsersModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string LatestMessage { get; set; }
        public bool? IsRead { get; set; }
        public DateTime Time { get; set; }
        public int ItemsPurchased { get; set; }
        public string Details { get; set; }
        public double ReviewAvgStars { get; set; }
    }
}
