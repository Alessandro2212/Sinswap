using Nop.Web.Models.MiniVendors;

namespace Nop.Web.Factories
{
    public partial interface IMiniVendorModelFactory
    {
        TopMiniVendorModel PrepareTopMiniVendorModel(int amount);

        TopMiniVendorModel PrepareMostPopularMiniVendorModel(int amount);

        TopMiniVendorModel PrepareTopCategoryMiniVendorModel(int categoryId, int amount);

        TopMiniVendorModel PrepareCategoryMiniVendorModel(int categoryId);

    }
}