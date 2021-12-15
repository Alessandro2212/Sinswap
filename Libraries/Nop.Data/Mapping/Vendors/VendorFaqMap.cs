using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Vendors;

namespace Nop.Data.Mapping.Vendors
{
    public partial class VendorFaqMap : NopEntityTypeConfiguration<VendorFaq>
    {
        #region Methods
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<VendorFaq> builder)
        {
            builder.ToTable(nameof(VendorFaq));
            builder.HasKey(record => record.Id);
        }
        #endregion
    }
}
