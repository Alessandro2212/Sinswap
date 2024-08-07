﻿using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

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
            var model = _faqModelFactory.PrepareFaqViewModel();

            return View(model);
        }
    }
}
