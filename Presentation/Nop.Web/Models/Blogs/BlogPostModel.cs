using System;
using System.Collections.Generic;
using FluentValidation.Attributes;
using Nop.Core.Domain.Blogs;
using Nop.Web.Framework.Models;
using Nop.Web.Validators.Blogs;

namespace Nop.Web.Models.Blogs
{
    [Validator(typeof(BlogPostValidator))]
    public partial class BlogPostModel : BaseNopEntityModel
    {
        public BlogPostModel()
        {
            Tags = new List<string>();
            Comments = new List<BlogCommentModel>();
            AddNewComment = new AddBlogCommentModel();
            RelatedBlogs = new List<BlogPost>();
        }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string MetaTitle { get; set; }
        public string SeName { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public string BodyOverview { get; set; }
        public bool AllowComments { get; set; }
        public int NumberOfComments { get; set; }
        public DateTime CreatedOn { get; set; }
        public string PictureUrl { get; set; }
        public string PictureCredit { get; set; }
        public string BlogUserCategory { get; set; }

        public IList<string> Tags { get; set; }

        public IList<BlogCommentModel> Comments { get; set; }
        public AddBlogCommentModel AddNewComment { get; set; }
        public IList<BlogPost> RelatedBlogs { get; set; }
    }
}