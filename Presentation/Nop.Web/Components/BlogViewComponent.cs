using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class BlogViewComponent : NopViewComponent
    {
        private readonly IBlogModelFactory _blogModelFactory;

        public BlogViewComponent(IBlogModelFactory blogModelFactory)
        {
            this._blogModelFactory = blogModelFactory;
        }

        public IViewComponentResult Invoke(int amount)
        {
            var model = _blogModelFactory.PrepareBlogModel(amount);
            if (model == null)
                return Content("");

            return View(model);
        }
    }
}