using System.Collections.Generic;

namespace Nop.Core.Domain.Catalog
{
    public partial class CategoryTag : BaseEntity
    {
        private ICollection<CategoryCategoryTagMapping> _categoryCategoryTagMappings;

        public string Tag { get; set; }

        public virtual ICollection<CategoryCategoryTagMapping> CategoryCategoryTagMappings
        {
            get => _categoryCategoryTagMappings ?? (_categoryCategoryTagMappings = new List<CategoryCategoryTagMapping>());
            protected set => _categoryCategoryTagMappings = value;
        }
    }
}