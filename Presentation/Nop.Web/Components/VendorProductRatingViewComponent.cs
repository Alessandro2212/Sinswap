using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class VendorProductRatingViewComponent : NopViewComponent
    {
        private readonly IVendorProductRatingModelFactory _vendorProductRatingModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorProductRatingViewComponent(IVendorProductRatingModelFactory vendorProductRatingModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorProductRatingModelFactory = vendorProductRatingModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(int vendorId)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _vendorProductRatingModelFactory.GetProductRatingByVendor(vendorId);
            if (model == null)
                return Content("");

            return View(model);
        }
    }
}
