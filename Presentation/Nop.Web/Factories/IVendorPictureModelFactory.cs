using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorPictureModelFactory
    {
        VendorPictureModel GetAllVendorPictures(int vendorId, int size);
    }
}
