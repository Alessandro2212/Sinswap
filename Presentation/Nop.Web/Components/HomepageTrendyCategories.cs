using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Components
{
    public class HomepageTrendyCategoriesViewComponent : NopViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public HomepageTrendyCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(int amount)
        {
            List<CategorySimpleModel> model;

            model = _catalogModelFactory.GetTrendyHomePageCategories(amount);

            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
