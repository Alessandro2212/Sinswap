using Nop.Core.Domain.Customers;

namespace Nop.Core.Domain.Vendors
{
    public partial class Follower : BaseEntity
    {
        public int VendorId { get; set; }

        public int CustomerId { get; set; }

        public virtual Vendor Vendor { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
