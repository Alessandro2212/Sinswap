using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.MiniVendors;

namespace Nop.Web.Components
{
    public class CategoryVendorViewComponent : NopViewComponent
    {
        private readonly IMiniVendorModelFactory _miniVendorModelFactory;
        private readonly VendorSettings _vendorSettings;

        public CategoryVendorViewComponent(IMiniVendorModelFactory miniVendorModelFactory,
            VendorSettings vendorSettings)
        {
            this._miniVendorModelFactory = miniVendorModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(int categoryId, string categoryName, TopMiniVendorModel modelAlreadyReady = null)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            if (modelAlreadyReady != null)
            {
                return View(new CategoryMiniVendorModel { MiniVendors = modelAlreadyReady.MiniVendors, Category = categoryName });
            }

            var vendors = _miniVendorModelFactory.PrepareCategoryMiniVendorModel(categoryId);
            var model = new CategoryMiniVendorModel { MiniVendors = vendors.MiniVendors, Category = categoryName };

            return View(model);
        }
    }
}