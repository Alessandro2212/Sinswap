using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Web.Components
{
    public class HomepageCategoriesViewComponent : NopViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public HomepageCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(bool showMostPopular = false)
        {
            List<CategorySimpleModel> model;

            if(showMostPopular)
                model = _catalogModelFactory.GetPopularHomePageCategories(8);
            else
                model = _catalogModelFactory.PrepareCategorySimpleModels();

            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
