using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorProductRatingModelFactory
    {
        VendorProductRatingModel GetProductRatingByVendor(int vendorId);
    }
}