using Microsoft.AspNetCore.Mvc;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Chat;

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
            ChatUsersViewModel model = null;
            //try to seek for the customer id given the vendor id. in case is not found it means that we are already supplying the 
            //correct id (the id is already from a customer) and we don't have to retrieve it
            if (userId == 0)
            {
                model = _chatModelFactory.GetChatUsersViewModel(vendorId, string.Empty);
            }
            else
            {
                model = _chatModelFactory.GetChatUsersViewModel(userId, string.Empty);
            }
            return View(model);
        }
    }
}
