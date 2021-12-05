using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Vendors
{
    public class VendorProductModel : BaseNopModel
    {
        public string Name { get; set; }
        public string SeName { get; set; }
        public string Price { get; set; }
        public PictureModel DefaultPictureModel { get; set; }
    }
}