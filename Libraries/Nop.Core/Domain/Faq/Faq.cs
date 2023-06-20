using System;

namespace Nop.Core.Domain.Faq
{
    public partial class Faq : BaseEntity
    {
        /// <summary>
        /// Gets or sets the customer identifier
        /// </summary>
        public int CategoryFaqId { get; set; }

        public string QuestionText { get; set; }

        public string ReplyText { get; set; }

        public int? NumberOfReadings { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets the vendor
        /// </summary>
        public virtual CategoryFaq CategoryFaq { get; set; }
    }
}