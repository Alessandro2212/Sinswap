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
        private readonly IVendorPictureModelFactory _vendorPictureModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorPictureViewComponent(IVendorPictureModelFactory vendorPictureModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorPictureModelFactory = vendorPictureModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(int vendorId, int size)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _vendorPictureModelFactory.GetAllVendorPictures(vendorId, size);
            if (model == null)
                return Content("");

            return View(model);
        }
    }
}