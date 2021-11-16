using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorSmallTalkViewComponent : NopViewComponent
    {
        private readonly IVendorReviewModelFactory _vendorReviewModelFactory;

        public VendorSmallTalkViewComponent(IVendorReviewModelFactory vendorReviewModelFactory)
        {
            this._vendorReviewModelFactory = vendorReviewModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId, int amount)
        {
            var model = _vendorReviewModelFactory.GetVendorSmallTalk(vendorId, amount);
            if (model == null || !model.VendorQuestions.Any())
                return Content("");

            return View(model);
        }
    }
}
