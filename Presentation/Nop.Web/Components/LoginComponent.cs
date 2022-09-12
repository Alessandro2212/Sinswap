using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class LoginViewComponent : NopViewComponent
    {
        private readonly ICustomerModelFactory _customerModelFactory;

        public LoginViewComponent(ICustomerModelFactory customerModelFactory)
        {
            this._customerModelFactory = customerModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _customerModelFactory.PrepareLoginModel(false);
            return View(model);
        }
    }
}
