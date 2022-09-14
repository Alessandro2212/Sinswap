using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class PasswordRecoveryViewComponent : NopViewComponent
    {
        private readonly ICustomerModelFactory _customerModelFactory;

        public PasswordRecoveryViewComponent(ICustomerModelFactory customerModelFactory)
        {
            this._customerModelFactory = customerModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _customerModelFactory.PreparePasswordRecoveryModel();

            return View(model);
        }
    }
}
