using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class PremiumVendorStoryModel : BaseNopModel
    {
        public IEnumerable<VendorStoryModel> VendorStories { get; set; }

        public string CustomerQuestion { get; set; }

        public int VendorId { get; set; }

        public string VendorSeName { get; set; }
    }
}