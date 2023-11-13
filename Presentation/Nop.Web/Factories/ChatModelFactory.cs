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
            var ids = chats.Select(x => x.FromId).Distinct().ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);

            var vendors = this._vendorService.GetVendorsFromReviewsAndCustomer(userId);

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

                    var vendorToRemove = vendors.ToList().Where(v => v.Id == vendor.Id).FirstOrDefault();
                    if (vendorToRemove != null)
                    {
                        vendors.ToList().Remove(vendorToRemove);
                    }
                }
                else
                {
                    chatUsersModel.Id = customer.Id;
                    chatUsersModel.Name = customer.Username;
                }
                chatUsersModel.LatestMessage = chat.Message;
                chatUsersModel.Time = chat.CreatedOnUtc;
                chatUsersModel.IsRead = chat.IsRead;
                chatUserModels.Add(chatUsersModel);
            }

            //to be tested (extra vendors in chat because of reviews)
            foreach (var vendor in vendors)
            {
                ChatUsersModel chatUsersModel = new ChatUsersModel();
                chatUsersModel.Id = vendor.Id;
                chatUsersModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 10);
                chatUsersModel.Name = vendor.Name;
                chatUserModels.Add(chatUsersModel);
            }

            return new ChatUsersViewModel() { ChatUsersModels = chatUserModels, CustomerId = userId };
        }

        public ChatConversationsViewModel GetChatConversationsViewModel(int userId, int partnerId)
        {
            var chats = _chatService.GetChatsOfUser(userId, partnerId);
            List<ChatConversationsModel> chatUsersModels = new List<ChatConversationsModel>();
            var ids = chats.Select(x => x.FromId).ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);
            ChatUsersModel chatUsersModel = new ChatUsersModel { Id = partnerId };
            foreach (var chat in chats)
            {
                ChatConversationsModel chatConversationsModel = new ChatConversationsModel();
                chatConversationsModel.Message = chat.Message;
                chatConversationsModel.Time = chat.CreatedOnUtc;
                var customer = customers.Where(x => x.Id == chat.FromId).FirstOrDefault();
                if (customer.Id == userId)
                {
                    chatConversationsModel.IsCurrentUser = true;
                }

                if (customer.VendorId > 0)
                {
                    var vendor = _vendorService.GetVendorById(customer.VendorId);
                    chatConversationsModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 10);
                    chatConversationsModel.Name = vendor.Name;
                    if (customer.Id == partnerId)
                    {
                        chatUsersModel.Name = chatConversationsModel.Name;
                        chatUsersModel.PictureUrl = chatConversationsModel.PictureUrl;
                    }
                }
                else
                {
                    chatConversationsModel.Name = customer.Username;
                    if (customer.Id == partnerId)
                    {
                        chatUsersModel.Name = customer.Username;
                    }
                }
                chatUsersModels.Add(chatConversationsModel);
            }

            //set the messages are read
            chats.ForEach(chat => { chat.IsRead = true; });
            _chatService.UpdateChatsAsRead(chats);

            return new ChatConversationsViewModel() { ChatUsersModels = chatUsersModels, ChatUsersModel = chatUsersModel, CurrentUserId = userId };
        }
    }
}
