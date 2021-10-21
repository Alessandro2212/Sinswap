using Nop.Core.Data;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Vendors;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Models.MiniVendors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the mini vendor model factory
    /// </summary>
    public partial class MiniVendorModelFactory : IMiniVendorModelFactory
    {
        private readonly IPictureService _pictureService;
        private readonly IVendorService _vendorService;
        private readonly IRepository<VendorPictureRecord> _vendorPictureRecordRepository;
        private readonly IUrlRecordService _urlRecordService;

        public MiniVendorModelFactory(
           IPictureService pictureService,
           IVendorService vendorService,
           IRepository<VendorPictureRecord> vendorPictureRecordRepository,
           IUrlRecordService urlRecordService)
        {
            this._pictureService = pictureService;
            this._vendorService = vendorService;
            this._vendorPictureRecordRepository = vendorPictureRecordRepository;
            this._urlRecordService = urlRecordService;
        }

        public TopMiniVendorModel PrepareTopCategoryMiniVendorModel(int amount)
        {
            //query to retrieve the top vendors of a specific category

            throw new NotImplementedException();
        }

        /// <summary>
        /// query to retriever the top (best) vendors of the home page
        /// </summary>
        /// <returns></returns>
        public TopMiniVendorModel PrepareTopMiniVendorModel(int amount)
        {
            //get top X vendors basing on the reviews
            var vendors = _vendorService.GetAllTopXVendors(amount);

            if (!vendors.Any())
                return new TopMiniVendorModel();

            TopMiniVendorModel model = new TopMiniVendorModel();
            List<MiniVendorModel> miniVendors = new List<MiniVendorModel>();

            foreach (Vendor vendor in vendors.ToList())
            {
                var query = from pp in _vendorPictureRecordRepository.Table
                            where pp.VendorId == vendor.Id
                            orderby pp.DisplayOrder, pp.Id
                            select pp;

                var vendorPictures = query.ToList().FirstOrDefault();
                Picture picture = null;
                if (vendorPictures != null)
                    picture = _pictureService.GetPictureById(vendorPictures.PictureId);

                int age = 18;
                if (vendor.BirthDate != null)
                {
                    age = DateTime.Today.Year - vendor.BirthDate.Year;
                    // Go back to the year the person was born in case of a leap year
                    if (vendor.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
                }

                miniVendors.Add(new MiniVendorModel
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    City = vendor.City,
                    Country = vendor.Country,
                    FollowersNumber = vendor.FollowersNumber,
                    Age = age,
                    PictureUrl = _pictureService.GetPictureUrl(picture),
                    SeName = _urlRecordService.GetSeName(vendor)
                });
            }

            if (miniVendors.Any())
                model.MiniVendors = new List<MiniVendorModel>(miniVendors);

            return model;
        }

        public TopMiniVendorModel PrepareMostPopularMiniVendorModel(int amount)
        {
            //get top X vendors basing on the reviews
            var vendors = _vendorService.GetMostPopularVendors(amount);

            if (!vendors.Any())
                return new TopMiniVendorModel();

            TopMiniVendorModel model = new TopMiniVendorModel();
            List<MiniVendorModel> miniVendors = new List<MiniVendorModel>();

            foreach (Vendor vendor in vendors.ToList())
            {
                var query = from pp in _vendorPictureRecordRepository.Table
                            where pp.VendorId == vendor.Id
                            orderby pp.DisplayOrder, pp.Id
                            select pp;

                var vendorPictures = query.ToList().FirstOrDefault();
                Picture picture = null;
                if (vendorPictures != null)
                    picture = _pictureService.GetPictureById(vendorPictures.PictureId);

                int age = 18;
                if (vendor.BirthDate != null)
                {
                    age = DateTime.Today.Year - vendor.BirthDate.Year;
                    // Go back to the year the person was born in case of a leap year
                    if (vendor.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
                }

                miniVendors.Add(new MiniVendorModel
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    City = vendor.City,
                    Country = vendor.Country,
                    FollowersNumber = vendor.FollowersNumber,
                    Age = age,
                    PictureUrl = _pictureService.GetPictureUrl(picture),
                    SeName = _urlRecordService.GetSeName(vendor)
                });
            }

            if (miniVendors.Any())
                model.MiniVendors = new List<MiniVendorModel>(miniVendors);

            return model;
        }

    }
}