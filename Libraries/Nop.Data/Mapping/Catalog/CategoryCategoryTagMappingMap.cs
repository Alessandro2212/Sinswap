using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    /// <summary>
    /// Mapping class
    /// </summary>
    public partial class CategoryCategoryTagMappingMap : NopEntityTypeConfiguration<CategoryCategoryTagMapping>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CategoryCategoryTagMapping> builder)
        {
            builder.ToTable(NopMappingDefaults.Category_CategoryTagTable);
            builder.HasKey(cct => cct.Id);

            builder.HasOne(cct => cct.Category)
                .WithMany(c => c.CategoryCategoryTagMappings)
                .HasForeignKey(cct => cct.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(cct => cct.CategoryTag)
                .WithMany(ct => ct.CategoryCategoryTagMappings)
                .HasForeignKey(cct => cct.CategoryTagId)
                .OnDelete(DeleteBehavior.SetNull);

            base.Configure(builder);
        }

        #endregion
    }
}
