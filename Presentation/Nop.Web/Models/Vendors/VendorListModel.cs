using Nop.Web.Enums;
using Nop.Web.Models.MiniVendors;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class VendorListModel
    {
        public VendorPagingFilteringModel PagingFilteringContext { get; set; }

        public IEnumerable<MiniVendorModel> MiniVendors { get; set; }

        public VendorTypeEnum VendorType { get; set; }
    }
}
