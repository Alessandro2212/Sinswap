using Nop.Web.Models.MiniVendors;
using System.Collections.Generic;

namespace Nop.Web.Models.Catalog
{
    public class CategoryListModel
    {
        public CategoryPagingFilteringModel PagingFilteringContext { get; set; }

        public IEnumerable<CategorySimpleModel> Categories { get; set; }
    }
}
