using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class PremiumVendorFavouriteCustomerModel : BaseNopModel
    {
        public IEnumerable<VendorCustomerModel> VendorCustomers { get; set; }
    }
}