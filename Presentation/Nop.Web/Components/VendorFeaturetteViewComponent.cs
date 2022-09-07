using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class VendorFeaturetteViewComponent : NopViewComponent
    {
        private readonly IVendorModelFactory _vendorModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorFeaturetteViewComponent(IVendorModelFactory vendorModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorModelFactory = vendorModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(string name)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _vendorModelFactory.PrepareVendorFeaturetteModel(name);
            if (model == null)
                return Content("");

            return View(model);
        }
    }
}