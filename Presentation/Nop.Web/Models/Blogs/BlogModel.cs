using Nop.Core.Domain.Blogs;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Blogs
{
    public class BlogModel : BaseNopEntityModel
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Color { get; set; }
        public string PictureUrl { get; set; }
        public bool HasPicture { get; set; }

        public BlogModel(BlogPost blogPost)
        {
            this.Title = blogPost.Title;
            this.Category = blogPost.BlogPostCategory?.Name;
            this.Color = blogPost.BlogPostCategory?.Color;
            this.HasPicture = blogPost.BlogPictures?.Count > 0;
        }
    }
}
