using Nop.Core.Domain.Vendors;
using Nop.Web.Models.MiniVendors;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial interface IMiniVendorModelFactory
    {
        TopMiniVendorModel PrepareTopMiniVendorModel(int amount);

        TopMiniVendorModel PrepareMostPopularMiniVendorModel(int amount);

        TopMiniVendorModel PrepareTopCategoryMiniVendorModel(int categoryId, int amount);

        TopMiniVendorModel PrepareCategoryMiniVendorModel(int categoryId);

        TopMiniVendorModel PrepareTopMiniVendorModel(IEnumerable<Vendor> vendors);
    }
}