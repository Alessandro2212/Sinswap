using Microsoft.AspNetCore.Mvc;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class ChatUsersViewComponent : NopViewComponent
    {
        private readonly IChatModelFactory _chatModelFactory;
        private readonly ICustomerService _customerService;

        public ChatUsersViewComponent(IChatModelFactory chatModelFactory, ICustomerService customerService)
        {
            this._chatModelFactory = chatModelFactory;
            this._customerService = customerService;
        }

        public IViewComponentResult Invoke(int vendorId)
        {
            var userId = this._customerService.GetCustomerIdByVendorId(vendorId);
            var model = _chatModelFactory.GetChatUsersViewModel(userId);
            return View(model);
        }
    }
}
