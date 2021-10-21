using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class PremiumVendorProductModel : BaseNopModel
    {
        public IEnumerable<VendorProductModel> VendorProducts { get; set; }
    }
}