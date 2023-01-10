using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class CategoryTagMap : NopEntityTypeConfiguration<CategoryTag>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CategoryTag> builder)
        {
            builder.ToTable(nameof(CategoryTag));
            builder.HasKey(categoryTag => categoryTag.Id);

            builder.Property(categoryTag => categoryTag.Tag).HasMaxLength(50).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}