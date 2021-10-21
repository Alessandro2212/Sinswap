using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorProductModelFactory
    {
        PremiumVendorProductModel GetAllVendorProducts(int vendorId);
    }
}
