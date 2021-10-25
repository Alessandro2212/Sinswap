using Nop.Core.Data;
using Nop.Core.Domain.Vendors;
using Nop.Services.Media;
using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Factories
{
    public partial class VendorPictureModelFactory : IVendorPictureModelFactory
    {
        private readonly IVendorService _vendorService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IRepository<VendorPictureRecord> _vendorPictureRecordRepository;
        private readonly IPictureService _pictureService;

        public VendorPictureModelFactory(
           IVendorService vendorService,
           IProductModelFactory productModelFactory,
           IRepository<VendorPictureRecord> vendorPictureRecordRepository,
           IPictureService pictureService)
        {
            this._vendorService = vendorService;
            this._productModelFactory = productModelFactory;
            this._vendorPictureRecordRepository = vendorPictureRecordRepository;
            this._pictureService = pictureService;
        }

        public VendorPictureModel GetAllVendorPictures(int vendorId, int size)
        {
            //get vendor pictures
            var query = from pp in _vendorPictureRecordRepository.Table
                        where pp.VendorId == vendorId
                        orderby pp.DisplayOrder, pp.Id
                        select pp;

            var vendorPictures = query.ToList();

            List<string> pictureUrls = new List<string>();

            foreach (var vp in vendorPictures)
            {
                var pictureUrl = _pictureService.GetPictureUrl(vp.PictureId, size);
                pictureUrls.Add(pictureUrl);
            }
            var model = new VendorPictureModel { VendorPictureUrls = pictureUrls };
            return model;
        }
    }
}
