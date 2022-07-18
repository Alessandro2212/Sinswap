using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Blogs;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Blogs;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Components
{
    public class AboutUsViewComponent : NopViewComponent
    {
        private readonly IBlogModelFactory _blogModelFactory;

        public AboutUsViewComponent(IBlogModelFactory blogModelFactory)
        {
            this._blogModelFactory = blogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            IList<AboutUsModel> model;

            model = _blogModelFactory.GetAllAboutUs();

            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
