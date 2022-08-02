using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Core.Domain.Blogs;

namespace Nop.Data.Mapping.Blogs
{
    public partial class BlogPostPictureMap : NopEntityTypeConfiguration<BlogPostPicture>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<BlogPostPicture> builder)
        {
            builder.ToTable("BlogPost_Picture");
            builder.HasKey(blogPost => blogPost.Id);

            builder.HasOne(blogPost => blogPost.Picture)
                .WithMany()
                .HasForeignKey(blogPost => blogPost.PictureId)
                .IsRequired();


            builder.HasOne(comment => comment.BlogPost)
                .WithMany(blog => blog.BlogPictures)
                .HasForeignKey(comment => comment.BlogPostId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}