using Nop.Core.Domain.Customers;
using System;

namespace Nop.Core.Domain.Chats
{
    public partial class Chat : BaseEntity
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedOnUtc { get; set; }

        public virtual Customer From { get; set; }
        public virtual Customer To { get; set; }
    }
}
