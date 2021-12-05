using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Vendors;

namespace Nop.Data.Mapping.Vendors
{
    public partial class VendorCustomerMap : NopEntityTypeConfiguration<VendorCustomer>
    {
        #region Methods
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<VendorCustomer> builder)
        {
            builder.ToTable(nameof(VendorCustomer));
            builder.HasKey(record => record.Id);
        }
        #endregion
    }
}
