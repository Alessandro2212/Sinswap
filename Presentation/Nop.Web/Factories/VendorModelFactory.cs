using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Seo;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Models.MiniVendors;
using Nop.Web.Models.Vendors;
using Nop.Web.Models.Common;
using Nop.Web.Areas.Admin.Factories;

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
        private readonly ILocalizedModelFactory _localizedModelFactory;
        private readonly IAddressService _addressService;
        private readonly ICustomerService _customerService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IAddressAttributeModelFactory _addressAttributeModelFactory;


        #endregion

        #region Ctor

        public VendorModelFactory(CaptchaSettings captchaSettings,
            CommonSettings commonSettings,
            IAddressService addressService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IVendorAttributeParser vendorAttributeParser,
            IVendorAttributeService vendorAttributeService,
            ILocalizedModelFactory localizedModelFactory,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            VendorSettings vendorSettings,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            ICustomerService customerService,
            IBaseAdminModelFactory baseAdminModelFactory,
            IAddressAttributeModelFactory addressAttributeModelFactory)
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
            this._localizedModelFactory = localizedModelFactory;
            this._addressService = addressService;
            this._customerService = customerService;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._addressAttributeModelFactory = addressAttributeModelFactory;
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

        /// <summary>
        /// Prepare vendor model
        /// </summary>
        /// <param name="model">Vendor model</param>
        /// <param name="vendor">Vendor</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Vendor model</returns>
        public virtual VendorEditModel PrepareVendorModel(VendorEditModel model, Vendor vendor, bool excludeProperties = false)
        {
            Action<VendorLocalizedModel, int> localizedModelConfiguration = null;
            if (vendor != null)
            {
                //fill in model values from the entity
                //model = model ?? vendor.ToModel<Models.Vendors.VendorEditModel>();
                model = model ?? new Models.Vendors.VendorEditModel();

                model.Active = vendor.Active;
                model.BirthDate = vendor.BirthDate;
                model.City = vendor.City;
                model.CountryId = vendor.CountryId ?? 0;
                model.Description = vendor.Description;
                model.Email = vendor.Email;
                model.FollowersNumber = vendor.FollowersNumber ?? 0;
                model.Id = vendor.Id;
                model.Name = vendor.Name;
                model.PictureId = vendor.PictureId;
                model.ShopName = vendor.ShopName;
                model.FavouriteHobby = vendor.FavouriteHobby;
                model.FavouriteMovie = vendor.FavouriteMovie;
                model.FavouriteWear = vendor.FavouriteWear;
                model.DoesPartnerKnow = vendor.DoesPartnerKnow ?? false;
                model.FavouriteThing = vendor.FavouriteThing;
                model.FavouriteFood = vendor.FavouriteFood;
                model.FavouriteKink = vendor.FavouriteKink;
                model.Secrets = vendor.Secrets;
                model.AddVendorNoteMessage = vendor.VendorNotes?.FirstOrDefault()?.Note ?? string.Empty;
                model.PictureUrl = _pictureService.GetPictureUrl(model.PictureId);

                //define localized model configuration action
                localizedModelConfiguration = (locale, languageId) =>
                {
                    locale.Name = _localizationService.GetLocalized(vendor, entity => entity.Name, languageId, false, false);
                    locale.Description = _localizationService.GetLocalized(vendor, entity => entity.Description, languageId, false, false);
                    locale.MetaKeywords = _localizationService.GetLocalized(vendor, entity => entity.MetaKeywords, languageId, false, false);
                    locale.MetaDescription = _localizationService.GetLocalized(vendor, entity => entity.MetaDescription, languageId, false, false);
                    locale.MetaTitle = _localizationService.GetLocalized(vendor, entity => entity.MetaTitle, languageId, false, false);
                    locale.SeName = _urlRecordService.GetSeName(vendor, languageId, false, false);
                };

                //prepare associated customers
                PrepareAssociatedCustomerModels(model.AssociatedCustomers, vendor);

                //prepare nested search models
                PrepareVendorNoteSearchModel(model.VendorNoteSearchModel, vendor);                
            }

            //set default values for the new model
            if (vendor == null)
            {
                model.PageSize = 6;
                model.Active = true;
                model.AllowCustomersToSelectPageSize = true;
                model.PageSizeOptions = _vendorSettings.DefaultVendorPageSizeOptions;
            }

            //prepare localized models
            if (!excludeProperties)
                model.Locales = _localizedModelFactory.PrepareLocalizedModels(localizedModelConfiguration);

            //prepare model vendor attributes
            PrepareVendorAttributeModels(model.VendorAttributes, vendor);

            //prepare address model
            var address = _addressService.GetAddressById(vendor?.AddressId ?? 0);
            //if (!excludeProperties && address != null)
            //    model.Address = address.ToModel(model.Address);
            PrepareAddressModel(model.Address, address);

            PrepareCustomerFields(model, vendor, address);

            return model;
        }

        protected virtual void PrepareCustomerFields(VendorEditModel model, Vendor vendor, Address address)
        {
            var customer = this._customerService.GetCustomerByVendorId(vendor.Id);
            model.Phone = customer.Phone;
            model.CustomerId = customer.Id;
            model.Address1 = address.Address1;
            model.Address2 = address.Address2;
            model.ZipCode = address.ZipPostalCode;
            model.City = address.City;
            model.State = address.StateProvince?.Name;
            //model.Country = address.Country?.Name;
            model.Country = address.County;
            model.AddressId = address.Id;
        }

        /// <summary>
        /// Prepare vendor associated customer models
        /// </summary>
        /// <param name="models">List of vendor associated customer models</param>
        /// <param name="vendor">Vendor</param>
        protected virtual void PrepareAssociatedCustomerModels(IList<VendorAssociatedCustomerModel> models, Vendor vendor)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            var associatedCustomers = _customerService.GetAllCustomers(vendorId: vendor.Id);
            foreach (var customer in associatedCustomers)
            {
                models.Add(new VendorAssociatedCustomerModel
                {
                    Id = customer.Id,
                    Email = customer.Email
                });
            }
        }

        /// <summary>
        /// Prepare vendor note search model
        /// </summary>
        /// <param name="searchModel">Vendor note search model</param>
        /// <param name="vendor">Vendor</param>
        /// <returns>Vendor note search model</returns>
        protected virtual VendorNoteSearchModel PrepareVendorNoteSearchModel(VendorNoteSearchModel searchModel, Vendor vendor)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            searchModel.VendorId = vendor.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare vendor attribute models
        /// </summary>
        /// <param name="models">List of vendor attribute models</param>
        /// <param name="vendor">Vendor</param>
        protected virtual void PrepareVendorAttributeModels(IList<VendorEditModel.VendorAttributeModel> models, Vendor vendor)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            //get available vendor attributes
            var vendorAttributes = _vendorAttributeService.GetAllVendorAttributes();
            foreach (var attribute in vendorAttributes)
            {
                var attributeModel = new VendorEditModel.VendorAttributeModel
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _vendorAttributeService.GetVendorAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new VendorEditModel.VendorAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(attributeValueModel);
                    }
                }

                //set already selected attributes
                if (vendor != null)
                {
                    var selectedVendorAttributes = _genericAttributeService.GetAttribute<string>(vendor, NopVendorDefaults.VendorAttributes);
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.Checkboxes:
                            {
                                if (!string.IsNullOrEmpty(selectedVendorAttributes))
                                {
                                    //clear default selection
                                    foreach (var item in attributeModel.Values)
                                        item.IsPreSelected = false;

                                    //select new values
                                    var selectedValues = _vendorAttributeParser.ParseVendorAttributeValues(selectedVendorAttributes);
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
                                if (!string.IsNullOrEmpty(selectedVendorAttributes))
                                {
                                    var enteredText = _vendorAttributeParser.ParseValues(selectedVendorAttributes, attribute.Id);
                                    if (enteredText.Any())
                                        attributeModel.DefaultValue = enteredText[0];
                                }
                            }
                            break;
                        case AttributeControlType.Datepicker:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                        case AttributeControlType.FileUpload:
                        default:
                            //not supported attribute control types
                            break;
                    }
                }

                models.Add(attributeModel);
            }
        }

        /// <summary>
        /// Prepare address model
        /// </summary>
        /// <param name="model">Address model</param>
        /// <param name="address">Address</param>
        protected virtual void PrepareAddressModel(AddressModel model, Address address)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //set some of address fields as enabled and required
            model.CountryEnabled = true;
            model.StateProvinceEnabled = true;
            model.CountyEnabled = true;
            model.CityEnabled = true;
            model.StreetAddressEnabled = true;
            model.StreetAddress2Enabled = true;
            model.ZipPostalCodeEnabled = true;
            model.PhoneEnabled = true;
            model.FaxEnabled = true;

            //prepare available countries
            _baseAdminModelFactory.PrepareCountries(model.AvailableCountries);

            //prepare available states
            _baseAdminModelFactory.PrepareStatesAndProvinces(model.AvailableStates, model.CountryId);

            //prepare custom address attributes
            _addressAttributeModelFactory.PrepareCustomAddressAttributes(model.CustomAddressAttributes, address);
        }

        #endregion
    }
}