using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Stores;
using System;

namespace Nop.Core.Domain.Vendors
{
    public partial class VendorReviewRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string ReviewText { get; set; }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string ReplyText { get; set; }

        public bool CustomerNotifiedOfReply { get; set; }

        public bool IsApproved { get; set; }

        public int Rating { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }

        public bool? IsQuestion { get; set; }

        /// <summary>
        /// Gets or sets the date and time of vendor note creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets the vendor
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// Gets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// Gets the store
        /// </summary>
        public virtual Store Store { get; set; }
    }
}
