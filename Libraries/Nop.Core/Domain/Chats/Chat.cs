using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using System;

namespace Nop.Core.Domain.Chats
{
    public partial class Chat : BaseEntity
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public bool? IsRead { get; set; }
        public bool? IsDeleted { get; set; }
        public int? PictureId { get; set; }
        public int? ChatMessageStatusId { get; set; }

        public virtual Customer From { get; set; }
        public virtual Customer To { get; set; }
        public virtual Picture Picture { get; set; }
        public virtual ChatMessageStatus ChatMessageStatus { get; set; }
    }
}
