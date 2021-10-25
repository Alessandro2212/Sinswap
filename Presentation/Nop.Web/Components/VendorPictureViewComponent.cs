using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;

namespace Nop.Web.Components
{
    public class VendorPictureViewComponent : NopViewComponent
    {
        private readonly IVendorProductModelFactory _vendorProductModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorPictureViewComponent(IVendorProductModelFactory vendorProductModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorProductModelFactory = vendorProductModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(IEnumerable<string> vendorPictureUrls)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = new VendorPictureModel { VendorPictureUrls = vendorPictureUrls };

            return View(model);
        }
    }
}