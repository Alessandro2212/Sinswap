using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Blogs;

namespace Nop.Data.Mapping.Blogs
{
    public partial class AboutUsMap : NopEntityTypeConfiguration<AboutUs>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<AboutUs> builder)
        {
            builder.ToTable(nameof(AboutUs));

            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Title).IsRequired();
            builder.Property(attribute => attribute.Content).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}
