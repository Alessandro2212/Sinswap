using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Linq;

namespace Nop.Web.Components
{
    public class VendorFaqViewComponent : NopViewComponent
    {
        private readonly IVendorQuestionModelFactory _vendorQuestionModelFactory;

        public VendorFaqViewComponent(IVendorQuestionModelFactory vendorQuestionModelFactory)
        {
            this._vendorQuestionModelFactory = vendorQuestionModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId, int amount)
        {
            var model = _vendorQuestionModelFactory.GetVendorFaqs(vendorId, amount);
            if (model == null || !model.VendorFaqs.Any())
                return Content("");

            return View(model);
        }
    }
}
