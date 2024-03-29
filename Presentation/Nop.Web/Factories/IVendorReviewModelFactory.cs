﻿using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorReviewModelFactory
    {
        PremiumVendorReviewModel GetVendorReviews(int vendorId);
        PremiumVendorSmallTalkModel GetVendorSmallTalk(int vendorId, int amount);
        PremiumVendorFavouriteCustomerModel GetVendorFavouriteCustomers(int vendorId, int amount);
        PremiumVendorStoryModel GetVendorStories(int vendorId, int amount);
    }
}