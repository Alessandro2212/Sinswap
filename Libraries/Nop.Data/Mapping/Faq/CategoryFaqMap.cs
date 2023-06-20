using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Faq;

namespace Nop.Data.Mapping.Faq
{
    public partial class CategoryFaqMap : NopEntityTypeConfiguration<CategoryFaq>
    {
        #region Methods
        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CategoryFaq> builder)
        {
            builder.ToTable(nameof(CategoryFaq));
            builder.HasKey(record => record.Id);
        }
        #endregion
    }
}
