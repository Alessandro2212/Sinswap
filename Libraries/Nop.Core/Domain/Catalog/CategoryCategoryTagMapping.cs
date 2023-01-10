namespace Nop.Core.Domain.Catalog
{
    public partial class CategoryCategoryTagMapping : BaseEntity
    {
        public int CategoryId { get; set; }

        public int CategoryTagId { get; set; }

        public virtual Category Category { get; set; }

        public virtual CategoryTag CategoryTag { get; set; }
    }
}