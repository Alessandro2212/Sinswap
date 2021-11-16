using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorFavCustomerViewComponent : NopViewComponent
    {
        private readonly IVendorReviewModelFactory _vendorReviewModelFactory;

        public VendorFavCustomerViewComponent(IVendorReviewModelFactory vendorReviewModelFactory)
        {
            this._vendorReviewModelFactory = vendorReviewModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId, int amount)
        {
            var model = _vendorReviewModelFactory.GetVendorFavouriteCustomers(vendorId, amount);
            if (model == null || !model.VendorCustomers.Any())
                return Content("");

            return View(model);
        }
    }
}
