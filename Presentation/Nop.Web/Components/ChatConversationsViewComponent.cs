using Microsoft.AspNetCore.Mvc;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class ChatConversationsViewComponent : NopViewComponent
    {
        private readonly IChatModelFactory _chatModelFactory;
        private readonly ICustomerService _customerService;

        public ChatConversationsViewComponent(IChatModelFactory chatModelFactory, ICustomerService customerService)
        {
            this._chatModelFactory = chatModelFactory;
            this._customerService = customerService;
        }

        public IViewComponentResult Invoke(int vendorId, int partnerId)
        {
            var userId = this._customerService.GetCustomerIdByVendorId(vendorId);
            partnerId = this._customerService.GetCustomerIdByVendorId(partnerId);
            var model = _chatModelFactory.GetChatConversationsViewModel(userId, partnerId);
            return View(model);
        }
    }
}
