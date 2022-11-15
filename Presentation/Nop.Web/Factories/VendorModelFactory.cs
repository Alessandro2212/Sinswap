﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Services.Common;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Models.MiniVendors;
using Nop.Web.Models.Vendors;

namespace Nop.Web.Factories
{
    /// <summary>
    /// Represents the vendor model factory
    /// </summary>
    public partial class VendorModelFactory : IVendorModelFactory
    {
        #region Fields

        private readonly CaptchaSettings _captchaSettings;
        private readonly CommonSettings _commonSettings;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IVendorAttributeParser _vendorAttributeParser;
        private readonly IVendorAttributeService _vendorAttributeService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorService _vendorService;

        #endregion

        #region Ctor

        public VendorModelFactory(CaptchaSettings captchaSettings,
            CommonSettings commonSettings,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IVendorAttributeParser vendorAttributeParser,
            IVendorAttributeService vendorAttributeService,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            VendorSettings vendorSettings,
            IUrlRecordService urlRecordService,
            IVendorService vendorService)
        {
            this._captchaSettings = captchaSettings;
            this._commonSettings = commonSettings;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            this._vendorAttributeParser = vendorAttributeParser;
            this._vendorAttributeService = vendorAttributeService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
            this._vendorSettings = vendorSettings;
            this._urlRecordService = urlRecordService;
            this._vendorService = vendorService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare vendor attribute models
        /// </summary>
        /// <param name="vendorAttributesXml">Vendor attributes in XML format</param>
        /// <returns>List of the vendor attribute model</returns>
        protected virtual IList<VendorAttributeModel> PrepareVendorAttributes(string vendorAttributesXml)
        {
            var result = new List<VendorAttributeModel>();

            var vendorAttributes = _vendorAttributeService.GetAllVendorAttributes();
            foreach (var attribute in vendorAttributes)
            {
                var attributeModel = new VendorAttributeModel
                {
                    Id = attribute.Id,
                    Name = _localizationService.GetLocalized(attribute, x => x.Name),
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _vendorAttributeService.GetVendorAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var valueModel = new VendorAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = _localizationService.GetLocalized(attributeValue, x => x.Name),
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(valueModel);
                    }
                }

                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.Checkboxes:
                        {
                            if (!string.IsNullOrEmpty(vendorAttributesXml))
                            {
                                //clear default selection
                                foreach (var item in attributeModel.Values)
                                    item.IsPreSelected = false;

                                //select new values
                                var selectedValues = _vendorAttributeParser.ParseVendorAttributeValues(vendorAttributesXml);
                                foreach (var attributeValue in selectedValues)
                                    foreach (var item in attributeModel.Values)
                                        if (attributeValue.Id == item.Id)
                                            item.IsPreSelected = true;
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //do nothing
                            //values are already pre-set
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            if (!string.IsNullOrEmpty(vendorAttributesXml))
                            {
                                var enteredText = _vendorAttributeParser.ParseValues(vendorAttributesXml, attribute.Id);
                                if (enteredText.Any())
                                    attributeModel.DefaultValue = enteredText[0];
                            }
                        }
                        break;
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.FileUpload:
                    default:
                        //not supported attribute control types
                        break;
                }

                result.Add(attributeModel);
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the apply vendor model
        /// </summary>
        /// <param name="model">The apply vendor model</param>
        /// <param name="validateVendor">Whether to validate that the customer is already a vendor</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="vendorAttributesXml">Vendor attributes in XML format</param>
        /// <returns>The apply vendor model</returns>
        public virtual ApplyVendorModel PrepareApplyVendorModel(ApplyVendorModel model,
            bool validateVendor, bool excludeProperties, string vendorAttributesXml)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (validateVendor && _workContext.CurrentCustomer.VendorId > 0)
            {
                //already applied for vendor account
                model.DisableFormInput = true;
                model.Result = _localizationService.GetResource("Vendors.ApplyAccount.AlreadyApplied");
            }

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnApplyVendorPage;
            model.TermsOfServiceEnabled = _vendorSettings.TermsOfServiceEnabled;
            model.TermsOfServicePopup = _commonSettings.PopupForTermsOfServiceLinks;

            if (!excludeProperties)
            {
                model.Email = _workContext.CurrentCustomer.Email;
            }

            //vendor attributes
            model.VendorAttributes = PrepareVendorAttributes(vendorAttributesXml);

            return model;
        }

        /// <summary>
        /// Prepare the vendor info model
        /// </summary>
        /// <param name="model">Vendor info model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="overriddenVendorAttributesXml">Overridden vendor attributes in XML format; pass null to use VendorAttributes of vendor</param>
        /// <returns>Vendor info model</returns>
        public virtual VendorInfoModel PrepareVendorInfoModel(VendorInfoModel model,
            bool excludeProperties, string overriddenVendorAttributesXml = "")
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var vendor = _workContext.CurrentVendor;
            if (!excludeProperties)
            {
                model.Description = vendor.Description;
                model.Email = vendor.Email;
                model.Name = vendor.Name;
            }

            var picture = _pictureService.GetPictureById(vendor.PictureId);
            var pictureSize = _mediaSettings.AvatarPictureSize;
            model.PictureUrl = picture != null ? _pictureService.GetPictureUrl(picture, pictureSize) : string.Empty;

            //vendor attributes
            if (string.IsNullOrEmpty(overriddenVendorAttributesXml))
                overriddenVendorAttributesXml = _genericAttributeService.GetAttribute<string>(vendor, NopVendorDefaults.VendorAttributes);
            model.VendorAttributes = PrepareVendorAttributes(overriddenVendorAttributesXml);

            return model;
        }

        public virtual VendorListModel PrepareVendorListModel(IPagedList<Vendor> vendors, VendorPagingFilteringModel command)
        {
            if (vendors == null)
                throw new ArgumentNullException(nameof(vendors));

            VendorListModel model = new VendorListModel();
            List<MiniVendorModel> miniVendorModels = new List<MiniVendorModel>();

            foreach (var vendor in vendors)
            {
                MiniVendorModel miniVendorModel = new MiniVendorModel
                {
                    Id = vendor.Id,
                    Name = vendor.Name,
                    City = vendor.City,
                    Country = vendor.Country?.Name ?? string.Empty,
                    PictureUrl = _pictureService.GetPictureUrl(vendor.PictureId),
                    Age = vendor.BirthDate != null ? GetAge(vendor.BirthDate) : 0,
                    SeName = _urlRecordService.GetSeName(vendor)
                };
                miniVendorModels.Add(miniVendorModel);
            }

            model.MiniVendors = miniVendorModels;
            model.PagingFilteringContext = command == null ? new VendorPagingFilteringModel() : command;

            model.PagingFilteringContext.LoadPagedList(vendors);

            return model;
        }

        public virtual VendorFeaturetteModel PrepareVendorFeaturetteModel(string name)
        {
            var vendorRecords = this._vendorService.GetVendorFeaturette(name);
            VendorFeaturetteModel model = new VendorFeaturetteModel();
            if (vendorRecords.Any())
            {
                var vendor = vendorRecords.First().Vendor;
                var pictureId1 = vendor.PictureId;
                var pictureId2 = vendorRecords.Last().PictureId;
                model.Name = vendor.Name;
                model.Quote = vendor.VendorNotes?.FirstOrDefault()?.Note;
                model.City = vendor.City;
                model.Country = vendor.Country?.Name ?? string.Empty;
                model.Age = vendor.BirthDate != null ? GetAge(vendor.BirthDate) : 0;
                model.KnownFor = vendor.KnownFor;
                model.SeName = _urlRecordService.GetSeName(vendor);
                model.PictureUrl1 = _pictureService.GetPictureUrl(pictureId1);
                model.PictureUrl2 = _pictureService.GetPictureUrl(pictureId2);
                model.BestSoldProduct = this._vendorService.GetVendorMostSoldProduct(vendor.Id);
            }
             
            return model;
        }

        private int GetAge(DateTime birthDay)
        {
            int age = 18;

            age = DateTime.Today.Year - birthDay.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthDay.Date > DateTime.Today.AddYears(-age)) age--;

            return age;
        }



        #endregion
    }
}