using Nop.Web.Framework.Models;
using System;

namespace Nop.Web.Models.Vendors
{
    public class VendorStoryModel : BaseNopModel
    {
        public string CustomerName { get; set; }

        public string QuestionText{ get; set; }

        public string ReplyText { get; set; }

        public DateTime UpdatedOn { get; set; }

        public string PictureUrl { get; set; }

        public bool IsOwnStory { get; set; }

    }
}
