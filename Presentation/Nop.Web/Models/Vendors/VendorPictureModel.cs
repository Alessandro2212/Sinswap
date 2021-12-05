using Nop.Core.Domain.Vendors;
using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class VendorPictureModel : BaseNopModel
    {
        public IEnumerable<string> VendorPictureUrls { get; set; }
    }
}
