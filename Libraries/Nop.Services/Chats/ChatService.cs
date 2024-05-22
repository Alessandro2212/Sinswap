using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Nop.Core.Data;
using Nop.Core.Domain.Chats;
using Nop.Core.Domain.Media;
using Nop.Services.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Nop.Services.Chats
{
    public class ChatService : IChatService
    {
        private readonly IRepository<Chat> _chatRepository;
        private readonly IRepository<ChatMessageStatus> _chatStatusRepository;
        private readonly IDownloadService _downloadService;
        private readonly IPictureService _pictureService;

        public ChatService(
            IRepository<Chat> chatRepo,
            IRepository<ChatMessageStatus> chatStatusRepo,
            IDownloadService downloadService,
            IPictureService pictureService)
        {
            this._chatRepository = chatRepo;
            this._chatStatusRepository = chatStatusRepo;
            this._downloadService = downloadService;
            this._pictureService = pictureService;
        }

        public List<Chat> GetLatestChatsByUser(int userId, string status)
        {
            var chatIdsQuery = _chatRepository.Table
                .Where(o => o.FromId.Equals(userId) || o.ToId.Equals(userId))
                .Select(x => new
                {
                    x.Id,
                    x.FromId,
                    x.ToId
                })
                .GroupBy(g => new { g.FromId, g.ToId })
                .Select(k => new
                {
                    Id = k.Max(x => x.Id),
                    PartnerId = k.Key.FromId.Equals(userId) ? k.Key.ToId : k.Key.FromId,
                    //CreateTime = k.Max(x => x.CreateTime)
                })
                .OrderByDescending(d => d.Id)
                .AsQueryable();
            //.Distinct(i => i.PartnerId);  // <============= just remove dublicate

            IQueryable<Chat> chatListQuery = null;
            if (status == "All")
            {
                chatListQuery = from l in chatIdsQuery
                                join x in _chatRepository.Table on l.Id equals x.Id
                                where (x.IsDeleted == null || x.IsDeleted == false)
                                orderby x.CreatedOnUtc descending
                                select (new Chat
                                {
                                    Id = x.Id,
                                    FromId = l.PartnerId,
                                    Message = x.Message,
                                    IsRead = x.IsRead,
                                    IsDeleted = x.IsDeleted,
                                    CreatedOnUtc = x.CreatedOnUtc
                                });
            }
            else if (status == "Unread")
            {
                chatListQuery = from l in chatIdsQuery
                                join x in _chatRepository.Table on l.Id equals x.Id
                                where x.IsRead == false && (x.IsDeleted == false || x.IsDeleted == null)
                                orderby x.CreatedOnUtc descending
                                select (new Chat
                                {
                                    Id = x.Id,
                                    FromId = l.PartnerId,
                                    Message = x.Message,
                                    IsRead = x.IsRead,
                                    IsDeleted = x.IsDeleted,
                                    CreatedOnUtc = x.CreatedOnUtc
                                });
            }
            else if (status == "Trash")
            {
                chatListQuery = from l in chatIdsQuery
                                join x in _chatRepository.Table on l.Id equals x.Id
                                where x.IsDeleted == true
                                orderby x.CreatedOnUtc descending
                                select (new Chat
                                {
                                    Id = x.Id,
                                    FromId = l.PartnerId,
                                    Message = x.Message,
                                    IsRead = x.IsRead,
                                    IsDeleted = x.IsDeleted,
                                    CreatedOnUtc = x.CreatedOnUtc
                                });
            }

            var chatList = chatListQuery.ToList();
            return chatList;
        }

        public List<Chat> GetChatsOfUser(int userId, int partnerId)
        {
            var query = from chat in _chatRepository.Table
                        where (chat.FromId == userId && chat.ToId == partnerId) ||
                              (chat.ToId == userId && chat.FromId == partnerId)
                        orderby chat.CreatedOnUtc
                        select chat;
            return query.ToList();
        }

        public Chat SaveChatMessage(int userId, int partnerId, string message, IFormFile formFile)
        {
            var isTrashed = this.GetChatsOfUser(userId, partnerId).FirstOrDefault()?.IsDeleted == true;
            int? pictureId = null;
            if (formFile != null)
            {
                pictureId = (this.SaveChatMessagePicture(formFile))?.Id;
            }
            Chat chat = new Chat { FromId = userId, ToId = partnerId, Message = message, CreatedOnUtc = DateTime.Now, PictureId = pictureId, IsRead = true };
            if (!isTrashed)
            {
                _chatRepository.Insert(chat);
            }
            return chat;
        }

        public Picture SaveChatMessagePicture(IFormFile formFile)
        {
            var contentType = formFile.ContentType;
            var vendorPictureBinary = _downloadService.GetDownloadBits(formFile);
            var picture = _pictureService.InsertPicture(vendorPictureBinary, contentType, null);
            return picture;
        }

        public void DeleteChatMessage(int userId, int partnerId)
        {
            var messages = this.GetChatsOfUser(userId, partnerId);
            foreach (var message in messages)
            {
                message.IsDeleted = true;
            }
            //_chatRepository.Delete(messages);
            this.UpdateChats(messages);
        }

        public void ResumeChatMessage(int userId, int partnerId)
        {
            var messages = this.GetChatsOfUser(userId, partnerId);
            foreach (var message in messages)
            {
                message.IsDeleted = false;
            }
            //_chatRepository.Delete(messages);
            this.UpdateChats(messages);
        }

        public void UpdateChats(IEnumerable<Chat> chats)
        {
            _chatRepository.Update(chats);
        }

        public int GetChatMessageStatusId(string chatStatus)
        {
            var query = from cs in _chatStatusRepository.Table
                        where cs.Status == chatStatus
                        select cs;
            return query.FirstOrDefault()?.Id ?? 0;
        }
    }
}
