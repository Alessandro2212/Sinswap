using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Stores;
using System;

namespace Nop.Core.Domain.Vendors
{
    public partial class VendorCustomerStory : BaseEntity
    {
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        public bool IsApproved { get; set; }

        public int StoreId { get; set; }

        public int? PictureId { get; set; }

        public string QuestionText { get; set; }

        public string QuestionReply { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime UpdatedOnUtc { get; set; }

        public bool IsOwnStory { get; set; }

        /// <summary>
        /// Gets the picture
        /// </summary>
        public virtual Picture Picture { get; set; }

        /// <summary>
        /// Gets the store
        /// </summary>
        public virtual Store Store { get; set; }

        /// <summary>
        /// Gets the vendor
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// Gets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }
    }
}
