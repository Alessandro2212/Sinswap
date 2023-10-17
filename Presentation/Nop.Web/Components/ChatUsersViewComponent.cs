using Microsoft.AspNetCore.Mvc;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components
{
    public class ChatUsersViewComponent : NopViewComponent
    {
        private readonly IChatModelFactory _chatModelFactory;

        public ChatUsersViewComponent(IChatModelFactory chatModelFactory)
        {
            this._chatModelFactory = chatModelFactory;
        }

        public IViewComponentResult Invoke(int userId)
        {
            var model = _chatModelFactory.GetChatUsersViewModel(userId);
            return View(model);
        }
    }
}
