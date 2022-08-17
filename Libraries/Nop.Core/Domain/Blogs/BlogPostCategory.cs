namespace Nop.Core.Domain.Blogs
{
    public partial class BlogPostCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string UserCategory { get; set; }
    }
}