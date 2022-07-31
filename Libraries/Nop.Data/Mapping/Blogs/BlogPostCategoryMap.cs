using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Blogs;

namespace Nop.Data.Mapping.Blogs
{
    public partial class BlogPostCategoryMap : NopEntityTypeConfiguration<BlogPostCategory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<BlogPostCategory> builder)
        {
            builder.ToTable("BlogPost_Category");
            builder.HasKey(blogPost => blogPost.Id);

            builder.Property(blogPost => blogPost.Name).IsRequired();
            builder.Property(blogPost => blogPost.Color).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}