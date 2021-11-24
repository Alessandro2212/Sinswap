using Nop.Services.Media;
using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial class VendorReviewModelFactory : IVendorReviewModelFactory
    {
        private readonly IVendorService _vendorService;
        private readonly IPictureService _pictureService;

        public VendorReviewModelFactory(IVendorService vendorService, IPictureService pictureService)
        {
            this._vendorService = vendorService;
            this._pictureService = pictureService;
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

        public PremiumVendorFavouriteCustomerModel GetVendorFavouriteCustomers(int vendorId, int amount)
        {
            //call the service
            var vendorCustomers = this._vendorService.GetVendorFavouriteCustomers(vendorId, amount);

            //prepare model for the view
            PremiumVendorFavouriteCustomerModel premiumVendorFavouriteCustomerModel = new PremiumVendorFavouriteCustomerModel();
            List<VendorCustomerModel> vendorCustomerModels = new List<VendorCustomerModel>();
            foreach (var vc in vendorCustomers)
            {
                var vendorQuestion = new VendorCustomerModel();
                vendorQuestion.CustomerName = vc.Customer?.Username;
                vendorCustomerModels.Add(vendorQuestion);
            }

            premiumVendorFavouriteCustomerModel.VendorCustomers = vendorCustomerModels;

            return premiumVendorFavouriteCustomerModel;
        }

        public PremiumVendorStoryModel GetVendorStories(int vendorId, int amount)
        {
            //call the service
            var vendorStories = this._vendorService.GetVendorStories(vendorId, amount);

            //prepare model for the view
            PremiumVendorStoryModel premiumVendorStoryModel = new PremiumVendorStoryModel();
            List<VendorStoryModel> vendorCustomerStoryModels = new List<VendorStoryModel>();
            foreach (var vs in vendorStories)
            {
                var vendorStory = new VendorStoryModel();
                vendorStory.CustomerName = vs.Customer.Username;
                vendorStory.UpdatedOn = vs.UpdatedOnUtc;
                vendorStory.QuestionText = vs.QuestionText;
                vendorStory.ReplyText = vs.QuestionReply;
                vendorStory.PictureUrl = vs.Picture == null ? string.Empty : this._pictureService.GetPictureUrl(vs.Picture);
                vendorStory.IsOwnStory = vs.IsOwnStory;

                vendorCustomerStoryModels.Add(vendorStory);
            }

            premiumVendorStoryModel.VendorStories = vendorCustomerStoryModels;

            return premiumVendorStoryModel;
        }
    }
}
