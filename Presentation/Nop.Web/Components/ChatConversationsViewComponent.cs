using Microsoft.AspNetCore.Mvc;
using Nop.Services.Customers;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Chat;
using System.Collections.Generic;

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
            if (partnerId <= 0)
            {
                var mod = new ChatConversationsViewModel() { ChatUsersModels = new List<ChatConversationsModel>(), ChatUsersModel = new ChatUsersModel() };
                return View(mod);
            }
            //try to seek for the customer id given the vendor id. in case is not found it means that we are already supplying the 
            //correct id (the id is already from a customer) and we don't have to retrieve it
            var userId = this._customerService.GetCustomerIdByVendorId(vendorId);
            var pId = this._customerService.GetCustomerIdByVendorId(partnerId);
            var model = _chatModelFactory.GetChatConversationsViewModel(userId == 0 ? vendorId : userId, pId == 0 ? partnerId : pId);
            return View(model);
        }
    }
}
