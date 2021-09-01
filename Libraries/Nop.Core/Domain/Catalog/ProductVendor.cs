using Nop.Core.Domain.Vendors;

namespace Nop.Core.Domain.Catalog
{
    public partial class ProductVendor: BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the vendor identifier
        /// </summary>
        public int VendorId { get; set; }

        /// <summary>
        /// Gets the vendor
        /// </summary>
        public virtual Vendor Vendor { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
