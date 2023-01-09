using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;
using Nop.Web.Models.NotFound;

namespace Nop.Web.Components
{
    public class VendorCategoryNotFoundViewComponent : NopViewComponent
    {
        public IViewComponentResult Invoke(string category)
        {
            var model = new VendorCategoryNotFoundModel { Category = category };
            return View(model);
        }
    }
}
