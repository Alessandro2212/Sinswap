using Nop.Core.Domain.Customers;
using Nop.Services.Chats;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Vendors;
using Nop.Web.Models.Chat;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Factories
{
    public class ChatModelFactory : IChatModelFactory
    {
        private readonly IChatService _chatService;
        private readonly IPictureService _pictureService;
        private readonly ICustomerService _customerService;
        private readonly IVendorService _vendorService;

        public ChatModelFactory(IChatService chatService,
            IPictureService pictureService,
            ICustomerService customerService,
            IVendorService vendorService)
        {
            this._chatService = chatService;
            this._pictureService = pictureService;
            this._customerService = customerService;
            this._vendorService = vendorService;
        }

        public ChatUsersViewModel GetChatUsersViewModel(int userId)
        {
            var chats = _chatService.GetLatestChatsByUser(userId);
            var ids = chats.Select(x => x.FromId).ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);

            List<ChatUsersModel> chatUserModels = new List<ChatUsersModel>();
            foreach (var customer in customers)
            {
                var chat = chats.Where(x => x.FromId == customer.Id).FirstOrDefault();
                ChatUsersModel chatUsersModel = new ChatUsersModel();
                if (customer.VendorId > 0)
                {
                    var vendor = _vendorService.GetVendorById(customer.VendorId);
                    chatUsersModel.Id = customer.VendorId;
                    chatUsersModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 10);
                    chatUsersModel.Name = vendor.Name;
                }
                else
                {
                    chatUsersModel.Id = customer.Id;
                    chatUsersModel.Name = customer.Username;
                }
                chatUsersModel.LatestMessage = chat.Message;
                chatUsersModel.Time = chat.CreatedOnUtc;
                chatUserModels.Add(chatUsersModel);
            }

            return new ChatUsersViewModel() { ChatUsersModels = chatUserModels };
        }


        public ChatConversationsViewModel GetChatConversationsViewModel(int userId, int partnerId)
        {
            var chats = _chatService.GetChatsOfUser(userId, partnerId);
            List<ChatConversationsModel> chatUsersModels = new List<ChatConversationsModel>();
            var ids = chats.Select(x => x.FromId).ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);

            foreach (var chat in chats)
            {
                ChatConversationsModel chatConversationsModel = new ChatConversationsModel();
                chatConversationsModel.Message = chat.Message;
                var customer = customers.Where(x => x.Id == chat.FromId).FirstOrDefault();
                if (customer.VendorId > 0)
                {
                    var vendor = _vendorService.GetVendorById(customer.VendorId);
                    chatConversationsModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 10);
                    chatConversationsModel.Name = vendor.Name;
                }
                else
                {
                    chatConversationsModel.Name = customer.Username;
                }
            }

            return new ChatConversationsViewModel() { ChatUsersModels = chatUsersModels };
        }
    }
}
