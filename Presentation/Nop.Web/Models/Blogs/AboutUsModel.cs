using Nop.Core.Domain.Blogs;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Blogs
{
    public class AboutUsModel : BaseNopEntityModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public AboutUsModel()
        {

        }

        public AboutUsModel(AboutUs us)
        {
            this.Id = us.Id;
            this.Title = us.Title;
            this.Content = us.Content;
        }
    }
}
