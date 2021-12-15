using Nop.Core.Domain.Customers;
using System;

namespace Nop.Core.Domain.Vendors
{
    public partial class VendorFaq : BaseEntity
    {
        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        public string QuestionText { get; set; }

        public string ReplyText { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

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