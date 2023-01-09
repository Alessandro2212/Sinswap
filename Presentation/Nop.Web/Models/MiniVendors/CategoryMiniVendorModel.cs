using System.Collections.Generic;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.MiniVendors
{
    public partial class CategoryMiniVendorModel : BaseNopModel
    {
        public CategoryMiniVendorModel()
        {
        }

        public int TotalVendors { get; set; }

        public List<MiniVendorModel> MiniVendors { get; set; }

        public string Category { get; set; }
    }
}