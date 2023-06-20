using Microsoft.EntityFrameworkCore;
using Nop.Core.Data;
using Nop.Core.Domain.Faq;
using Nop.Services.Media;
using Nop.Web.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the mini vendor model factory
    /// </summary>
    public partial class FaqModelFactory : IFaqModelFactory
    {
        private readonly IPictureService _pictureService;
        private readonly IRepository<CategoryFaq> _categoryFaqRepository;

        public FaqModelFactory(
           IPictureService pictureService,
           IRepository<CategoryFaq> categoryFaqRepository)
        {
            this._pictureService = pictureService;
            this._categoryFaqRepository = categoryFaqRepository;
        }

        public FaqViewModel PrepareFaqViewModel(IEnumerable<string> faqForFrequently)
        {
            FaqViewModel model = new FaqViewModel();
            var categoryFaqs = _categoryFaqRepository.Table.Include(c => c.Faqs).ToList();
            foreach (var ff in faqForFrequently)
            {
                var cfaq = categoryFaqs.Where(cf => cf.Name.Equals(ff)).First();
                model.FrequentCategoryFaqs.Add(cfaq);
                var faqs = categoryFaqs.Where(cf => cf.Name.Equals(ff))
                    .SelectMany(cf => cf.Faqs)
                    .OrderByDescending(f => f.NumberOfReadings)
                    .Take(2);

                foreach (var f in faqs)
                {
                    var catFaqModel = new CategoryFaqViewModel
                    {
                        CategoryFaqId = f.CategoryFaqId,
                        FaqId = f.Id,
                        Title = f.QuestionText
                    };
                    model.FrequentCategoryFaqViewModels.Add(catFaqModel);
                }
            }

            foreach (var catFaq in categoryFaqs)
            {
                var catFaqModel = new CategoryFaqViewModel
                {
                    CategoryFaqId = catFaq.Id,
                    Title = catFaq.Name,
                    SubTitle = catFaq.SubName,
                    PictureUrl = _pictureService.GetPictureUrl(catFaq.PictureId),
                };
                model.CategoryFaqViewModels.Add(catFaqModel);
            }

            return model;
        }
    }
}