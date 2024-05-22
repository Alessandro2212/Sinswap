using Nop.Core.Domain.Chats;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Vendors;
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
        private readonly IVendorProductRatingModelFactory _vendorProductRatingModelFactory;

        public ChatModelFactory(IChatService chatService,
            IPictureService pictureService,
            ICustomerService customerService,
            IVendorService vendorService,
            IVendorProductRatingModelFactory vendorProductRatingModelFactory)
        {
            this._chatService = chatService;
            this._pictureService = pictureService;
            this._customerService = customerService;
            this._vendorService = vendorService;
            this._vendorProductRatingModelFactory = vendorProductRatingModelFactory;
        }

        public ChatUsersViewModel GetChatUsersViewModel(int userId, string chatStatus)
        {
            List<Chat> chats = new List<Chat>();
            if (string.IsNullOrEmpty(chatStatus))
            {
                chats = _chatService.GetLatestChatsByUser(userId, "All");
            }
            else
            {
                chats = _chatService.GetLatestChatsByUser(userId, chatStatus);
            }
            var ids = chats.Select(x => x.FromId).Distinct().ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);
            var vendors = this._vendorService.GetVendorsFromReviewsAndCustomer(userId);
            var vendorsForPurchasedProducts = this._vendorService.GetVendorsFromCustomerPurchasedItems(userId);
            var visitedVendors = new List<int>();
            List<ChatUsersModel> chatUserModels = new List<ChatUsersModel>();
            foreach (var customer in customers)
            {
                var chat = chats.Where(x => x.FromId == customer.Id).FirstOrDefault();
                ChatUsersModel chatUsersModel = new ChatUsersModel();
                if (customer.VendorId > 0)
                {
                    var vendor = _vendorService.GetVendorById(customer.VendorId);

                    chatUsersModel.Id = customer.VendorId;
                    chatUsersModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 100);
                    chatUsersModel.Name = vendor.Name;
                    var rating = this._vendorProductRatingModelFactory.GetProductRatingByVendor(customer.VendorId);
                    chatUsersModel.ReviewAvgStars = rating.ReviewAvgStars;
                    visitedVendors.Add(vendor.Id);
                }
                else
                {
                    chatUsersModel.Id = customer.Id;
                    chatUsersModel.Name = customer.Username;
                    //TODO: load the pictures of non vendor users
                    //add more info about this non vendor profile which im chatting with
                    chatUsersModel.ItemsPurchased = this._vendorService.GetNumberOfPurchasedItems(customer.Id, userId);
                }
                chatUsersModel.LatestMessage = chat.Message;
                chatUsersModel.Time = chat.CreatedOnUtc;
                chatUsersModel.IsRead = chat.IsRead;
                chatUsersModel.Details = $"{customer.GetAge()}, {customer.City}, {customer.Country?.Name}";
                chatUserModels.Add(chatUsersModel);
            }

            //extra vendors in chat because of reviews or purchased product from
            if (string.IsNullOrEmpty(chatStatus) || chatStatus == "All")
            {
                foreach (var vendor in vendors.Union(vendorsForPurchasedProducts).Distinct())
                {
                    if (!visitedVendors.Contains(vendor.Id))
                    {
                        ChatUsersModel chatUsersModel = new ChatUsersModel();
                        chatUsersModel.Id = vendor.Id;
                        chatUsersModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 100);
                        chatUsersModel.Name = vendor.Name;
                        chatUserModels.Add(chatUsersModel);
                    }
                }
            }

            bool isTrash = chatStatus == "Trash";
            return new ChatUsersViewModel() { ChatUsersModels = chatUserModels, CustomerId = userId, isTrash = isTrash };
        }

        public ChatConversationsViewModel GetChatConversationsViewModel(int userId, int partnerId)
        {
            var chats = _chatService.GetChatsOfUser(userId, partnerId);
            List<ChatConversationsModel> chatUsersModels = new List<ChatConversationsModel>();
            var ids = chats.Select(x => x.FromId).ToArray();
            var customers = this._customerService.GetCustomersByIds(ids);
            ChatUsersModel chatUsersModel = new ChatUsersModel { Id = partnerId };
            //var partnerData = _vendorService.GetVendorById(partnerId);
            //if (partnerData != null)
            //{
            //    chatUsersModel.Name = partnerData.Name;
            //    chatUsersModel.PictureUrl = _pictureService.GetPictureUrl(partnerData.PictureId, 100);
            //}
            foreach (var chat in chats)
            {
                ChatConversationsModel chatConversationsModel = new ChatConversationsModel();
                chatConversationsModel.Message = chat.Message;
                if (chat.PictureId > 0)
                {
                    chatConversationsModel.MessagePictureUrl = _pictureService.GetPictureUrl(chat.PictureId.Value, 100);
                }
                chatConversationsModel.Time = chat.CreatedOnUtc;
                var customer = customers.Where(x => x.Id == chat.FromId).FirstOrDefault();
                if (customer.Id == userId)
                {
                    chatConversationsModel.IsCurrentUser = true;
                }

                if (customer.VendorId > 0)
                {
                    var vendor = _vendorService.GetVendorById(customer.VendorId);
                    chatConversationsModel.PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId, 100);
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
            _chatService.UpdateChats(chats);

            return new ChatConversationsViewModel() { ChatUsersModels = chatUsersModels, ChatUsersModel = chatUsersModel, CurrentUserId = userId };
        }
    }
}
