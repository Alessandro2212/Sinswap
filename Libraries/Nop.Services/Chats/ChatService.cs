﻿using Microsoft.AspNetCore.Http;
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
        private readonly IDownloadService _downloadService;
        private readonly IPictureService _pictureService;

        public ChatService(
            IRepository<Chat> chatRepo,
            IDownloadService downloadService,
            IPictureService pictureService)
        {
            this._chatRepository = chatRepo;
            this._downloadService = downloadService;
            this._pictureService = pictureService;
        }

        public List<Chat> GetLatestChatsByUser(int userId)
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

            //var chatListQuery = from l in chatIdsQuery
            //                    join x in _chatRepository.Table on l.Id equals x.Id
            //                    orderby x.CreatedOnUtc descending
            //                    select (new
            //                    {
            //                        x.Id,
            //                        l.PartnerId,
            //                        x.Message,
            //                        x.CreatedOnUtc
            //                    });

            var chatListQuery = from l in chatIdsQuery
                                join x in _chatRepository.Table on l.Id equals x.Id
                                orderby x.CreatedOnUtc descending
                                select (new Chat
                                {
                                    Id = x.Id,
                                    FromId = l.PartnerId,
                                    Message = x.Message,
                                    IsRead = x.IsRead,
                                    CreatedOnUtc = x.CreatedOnUtc
                                });
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
            int? pictureId = null;
            if (formFile != null)
            {
                pictureId = (this.SaveChatMessagePicture(formFile))?.Id;
            }
            Chat chat = new Chat { FromId = userId, ToId = partnerId, Message = message, CreatedOnUtc = DateTime.Now, PictureId = pictureId };
            _chatRepository.Insert(chat);
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
            _chatRepository.Delete(messages);
        }

        public void UpdateChatsAsRead(IEnumerable<Chat> chats)
        {
            _chatRepository.Update(chats);
        }
    }
}
