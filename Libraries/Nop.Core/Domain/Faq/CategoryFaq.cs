using Nop.Core.Domain.Media;
using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.Faq
{
    public partial class CategoryFaq : BaseEntity
    {
        public string Name { get; set; }

        public string SubName { get; set; }

        public int PictureId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public virtual Picture Picture { get; set; }

        public virtual ICollection<Faq> Faqs { get; set; }
    }
}