using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorProductViewComponent : NopViewComponent
    {
        private readonly IVendorProductModelFactory _vendorProductModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorProductViewComponent(IVendorProductModelFactory vendorProductModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorProductModelFactory = vendorProductModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(int vendorId)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _vendorProductModelFactory.GetAllVendorProducts(vendorId);
            if (model == null || !model.VendorProducts.Any())
                return Content("");

            return View(model);
        }
    }
}
