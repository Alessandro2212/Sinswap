using System;

namespace Nop.Core.Domain.Customers
{
    public class CustomerActivationCode : BaseEntity
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CustomerId { get; set; }

        public string ActivationCode { get; set; }

        public DateTime InsertAt { get; set; }

        /// <summary>
        /// Gets or sets the customer
        /// </summary>
        public virtual Customer Customer { get; set; }
    }
}
