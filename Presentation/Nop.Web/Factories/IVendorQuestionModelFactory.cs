using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    public partial interface IVendorQuestionModelFactory
    {
        VendorFaqViewModel GetVendorFaqs(int vendorId, int amount);
    }
}
