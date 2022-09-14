using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Customer;

namespace Nop.Web.Components
{
    public class RegisterViewComponent : NopViewComponent
    {
        private readonly ICustomerModelFactory _customerModelFactory;

        public RegisterViewComponent(ICustomerModelFactory customerModelFactory)
        {
            this._customerModelFactory = customerModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = new RegisterModel();
            model = _customerModelFactory.PrepareRegisterModel(model, false, setDefaultValues: true);

            return View(model);
        }
    }
}
