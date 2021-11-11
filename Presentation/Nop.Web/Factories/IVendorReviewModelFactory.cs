using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorReviewModelFactory
    {
        PremiumVendorReviewModel GetVendorReviews(int vendorId);
    }
}