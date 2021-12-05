using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorStoryViewComponent : NopViewComponent
    {
        private readonly IVendorReviewModelFactory _vendorReviewModelFactory;

        public VendorStoryViewComponent(IVendorReviewModelFactory vendorReviewModelFactory)
        {
            this._vendorReviewModelFactory = vendorReviewModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId, string vendorSeName, int amount)
        {
            if (this.User.Identity.IsAuthenticated == false)
                return Content("");

            var model = _vendorReviewModelFactory.GetVendorStories(vendorId, amount);
            if (model == null || !model.VendorStories.Any())
                return Content("");

            model.VendorId = vendorId;
            model.VendorSeName = vendorSeName;

            return View(model);
        }
    }
}
