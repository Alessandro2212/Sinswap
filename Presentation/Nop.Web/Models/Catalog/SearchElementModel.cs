using Nop.Web.Enums;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public class SearchElementModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SeName { get; set; }
        public PictureModel PictureModel { get; set; }
        public SearchElementEnum SearchElementEnum { get; set; }
    }
}
