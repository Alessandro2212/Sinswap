using Microsoft.AspNetCore.Mvc;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class VendorProductEditViewComponent : NopViewComponent
    {
        private readonly Areas.Admin.Factories.IProductModelFactory _productModelFactory;

        public VendorProductEditViewComponent(Areas.Admin.Factories.IProductModelFactory productModelFactory)
        {
            this._productModelFactory = productModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId)
        {
            var searchModel = new ProductSearchModel { SearchVendorId = vendorId };
            //var model = _productModelFactory.PrepareProductListModel(searchModel);

            //if (model == null || !model.VendorProducts.Any())
            //    return Content("");

            return View(searchModel);
        }
    }
}
