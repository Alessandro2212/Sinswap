using System;

namespace Nop.Core.Domain.Customers
{
    public class CustomerActivationCode : BaseEntity
    {
        public string ActivationCode { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerType { get; set; }
        public DateTime InsertAt { get; set; }
    }
}
