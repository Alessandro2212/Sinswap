using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

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

        public IViewComponentResult Invoke(int categoryId)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _miniVendorModelFactory.PrepareCategoryMiniVendorModel(categoryId);

            return View(model);
        }
    }
}