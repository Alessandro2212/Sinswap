using Nop.Core.Domain.Media;

namespace Nop.Core.Domain.Blogs
{
    /// <summary>
    /// Represents a blog post picture
    /// </summary>
    public partial class BlogPostPicture : BaseEntity
    {
        public int BlogPostId { get; set; }

        public int PictureId { get; set; }

        public string PictureCredit { get; set; }

        public virtual BlogPost BlogPost { get; set; }

        public virtual Picture Picture { get; set; }
    }
}