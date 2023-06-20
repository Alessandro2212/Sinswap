using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using System.Collections.Generic;

namespace Nop.Web.Components
{
    public class FaqViewComponent : NopViewComponent
    {
        private readonly IFaqModelFactory _faqModelFactory;

        public FaqViewComponent(IFaqModelFactory faqModelFactory)
        {
            this._faqModelFactory = faqModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var frequentCategories = new List<string> { "General", "Buyer", "Seller" };
            var model = _faqModelFactory.PrepareFaqViewModel(frequentCategories);

            return View(model);
        }
    }
}
