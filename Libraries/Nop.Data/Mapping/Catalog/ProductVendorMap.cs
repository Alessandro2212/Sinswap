using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product vendor mapping configuration
    /// </summary>
    public partial class ProductVendorMap : NopEntityTypeConfiguration<ProductVendor>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductVendor> builder)
        {
            builder.ToTable(NopMappingDefaults.ProductVendorTable);
            builder.HasKey(productVendor => productVendor.Id);

            builder.HasOne(productVendor => productVendor.Vendor)
                .WithMany()
                .HasForeignKey(productVendor => productVendor.VendorId)
                .IsRequired();

            builder.HasOne(productVendor => productVendor.Product)
                .WithMany(product => product.ProductVendors)
                .HasForeignKey(productVendor => productVendor.ProductId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}