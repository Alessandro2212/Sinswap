using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial class VendorReviewModelFactory : IVendorReviewModelFactory
    {
        private readonly IVendorService _vendorService;

        public VendorReviewModelFactory(IVendorService vendorService)
        {
            this._vendorService = vendorService;
        }

        public PremiumVendorReviewModel GetVendorReviews(int vendorId)
        {
            //call the service
            var vendorReviews = this._vendorService.GetVendorReviews(vendorId);

            //prepare model for the view
            PremiumVendorReviewModel premiumVendorReviewModel = new PremiumVendorReviewModel();
            List<VendorReviewModel> vendorReviewModels = new List<VendorReviewModel>();
            foreach (var vr in vendorReviews)
            {
                var vendorReview = new VendorReviewModel();
                vendorReview.Rating = vr.Rating;
                vendorReview.CustomerName = vr.Customer?.Username;
                vendorReview.CustomerCountry = vr.Customer?.Country?.Name;
                vendorReview.ReviewText = vr.ReviewText;
                vendorReview.CreatedOn = vr.CreatedOnUtc;
                vendorReviewModels.Add(vendorReview);
            }

            premiumVendorReviewModel.VendorReviews = vendorReviewModels;
            
            return premiumVendorReviewModel;
        }

        public PremiumVendorSmallTalkModel GetVendorSmallTalk(int vendorId, int amount)
        {
            //call the service
            var vendorQuestions = this._vendorService.GetVendorQuestions(vendorId, amount);

            //prepare model for the view
            PremiumVendorSmallTalkModel premiumVendorSmallTalkModel = new PremiumVendorSmallTalkModel();
            List<VendorQuestionModel> vendorQuestionModels = new List<VendorQuestionModel>();
            foreach (var vq in vendorQuestions)
            {
                var vendorQuestion = new VendorQuestionModel();
                vendorQuestion.Question = vq.ReviewText;
                vendorQuestion.Answer = vq.ReplyText;
                vendorQuestionModels.Add(vendorQuestion);
            }

            premiumVendorSmallTalkModel.VendorQuestions = vendorQuestionModels;

            return premiumVendorSmallTalkModel;
        }
    }
}
