using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class PremiumVendorReviewModel : BaseNopModel
    {
        public IEnumerable<VendorReviewModel> VendorReviews { get; set; }
    }
}