using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Vendors;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorReviewViewComponent : NopViewComponent
    {
        private readonly IVendorReviewModelFactory _vendorReviewModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorReviewViewComponent(IVendorReviewModelFactory vendorReviewModelFactory,
            VendorSettings vendorSettings)
        {
            this._vendorReviewModelFactory = vendorReviewModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke(int vendorId)
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _vendorReviewModelFactory.GetVendorReviews(vendorId);
            if (model == null || !model.VendorReviews.Any())
                return Content("");

            return View(model);
        }
    }
}
