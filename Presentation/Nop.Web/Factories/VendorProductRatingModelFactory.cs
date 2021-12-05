using Nop.Services.Orders;
using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System;
using System.Linq;

namespace Nop.Web.Factories
{
    public partial class VendorProductRatingModelFactory : IVendorProductRatingModelFactory
    {
        private readonly IVendorService _vendorService;
        private readonly IOrderService _orderService;

        public VendorProductRatingModelFactory(
           IVendorService vendorService,
           IOrderService orderService)
        {
            this._vendorService = vendorService;
            this._orderService = orderService;
        }

        public VendorProductRatingModel GetProductRatingByVendor(int vendorId)
        {
            var vendorSoldItems = this._orderService.GetNumberOfSoldProductsByVendor(vendorId);

            var vendorRatings = this._vendorService.GetVendorReviews(vendorId).ToList();

            int totalReviewRatings = 0;

            foreach (var vr in vendorRatings)
            {
                totalReviewRatings += vr.Rating;
            }

            double averageReviews = (double)totalReviewRatings / vendorRatings.Count();

            //prepare model for the view
            VendorProductRatingModel vendorProductRatingModel = new VendorProductRatingModel
            {
                NumberOfSoldProducts = vendorSoldItems,
                NumberOfReviews = vendorRatings.Count(),
                ReviewAvgStars = Math.Ceiling(averageReviews * 2) / 2
            };

            return vendorProductRatingModel;
        }
    }
}
