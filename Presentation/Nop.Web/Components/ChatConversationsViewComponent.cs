using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Chat;
using System.Collections.Generic;

namespace Nop.Web.Components
{
    public class ChatConversationsViewComponent : NopViewComponent
    {
        private readonly IChatModelFactory _chatModelFactory;

        public ChatConversationsViewComponent(IChatModelFactory chatModelFactory)
        {
            this._chatModelFactory = chatModelFactory;
        }

        public IViewComponentResult Invoke(int vendorId, int partnerId)
        {
            var model = _chatModelFactory.GetChatConversationsViewModel(vendorId, partnerId);
            return View(model);
        }
    }
}
