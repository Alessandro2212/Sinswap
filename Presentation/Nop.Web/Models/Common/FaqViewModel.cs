using Nop.Core.Domain.Faq;
using System.Collections.Generic;

namespace Nop.Web.Models.Common
{
    public class FaqViewModel
    {
        public FaqViewModel()
        {
            FrequentCategoryFaqs = new List<CategoryFaq>();
            FrequentCategoryFaqViewModels = new List<CategoryFaqViewModel>();
            CategoryFaqViewModels = new List<CategoryFaqViewModel>();
        }

        public List<CategoryFaq>  FrequentCategoryFaqs { get; set; }
        public List<CategoryFaqViewModel> FrequentCategoryFaqViewModels { get; set; }
        public List<CategoryFaqViewModel> CategoryFaqViewModels { get; set; }
    }
}
