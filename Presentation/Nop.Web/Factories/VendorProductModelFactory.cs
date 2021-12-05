using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial class VendorProductModelFactory : IVendorProductModelFactory
    {
        private readonly IVendorService _vendorService;
        private readonly IProductModelFactory _productModelFactory;

        public VendorProductModelFactory(
           IVendorService vendorService,
           IProductModelFactory productModelFactory)
        {
            this._vendorService = vendorService;
            this._productModelFactory = productModelFactory;
        }

        public PremiumVendorProductModel GetAllVendorProducts(int vendorId)
        {
            //call the service
            var vendorProducts = this._vendorService.GetAllVendorProducts(vendorId);

            //prepare model for the view
            PremiumVendorProductModel premiumVendorProductModel = new PremiumVendorProductModel();
            List<VendorProductModel> vendorProductModels = new List<VendorProductModel>();

            var overviewProd = this._productModelFactory.PrepareProductOverviewModels(vendorProducts);
            foreach (var vp in overviewProd)
            {
                var vpm = new VendorProductModel();
                vpm.DefaultPictureModel = vp.DefaultPictureModel;
                vpm.Name = vp.Name; //capire se prendere il name o seName
                vpm.SeName = vp.SeName;
                vpm.Price = vp.ProductPrice.Price;

                vendorProductModels.Add(vpm);
            }

            premiumVendorProductModel.VendorProducts = vendorProductModels;

            return premiumVendorProductModel;
        }
    }
}
