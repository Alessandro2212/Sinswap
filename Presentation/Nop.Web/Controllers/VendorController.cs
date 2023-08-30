using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Vendors;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;
using Nop.Web.Framework.Security.Captcha;
using Nop.Web.Models.Vendors;

namespace Nop.Web.Controllers
{
    public partial class VendorController : BasePublicController
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly CaptchaSettings _captchaSettings;
        private readonly ICustomerService _customerService;
        private readonly IDownloadService _downloadService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorAttributeParser _vendorAttributeParser;
        private readonly IVendorAttributeService _vendorAttributeService;
        private readonly IVendorModelFactory _vendorModelFactory;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly VendorSettings _vendorSettings;
        private readonly IPermissionService _permissionService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly Areas.Admin.Factories.IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IShippingService _shippingService;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IAclService _aclService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IDiscountService _discountService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;

        #endregion

        #region Ctor

        public VendorController(CaptchaSettings captchaSettings,
            IAddressService addressService,
            ICustomerService customerService,
            IDownloadService downloadService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IUrlRecordService urlRecordService,
            IVendorAttributeParser vendorAttributeParser,
            IVendorAttributeService vendorAttributeService,
            IVendorModelFactory vendorModelFactory,
            IVendorService vendorService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            VendorSettings vendorSettings,
            IPermissionService permissionService,
            ICustomerActivityService customerActivityService,
            ILocalizedEntityService localizedEntityService,
            Areas.Admin.Factories.IProductModelFactory productModelFactory,
            IProductService productService,
            IProductTagService productTagService,
            IShippingService shippingService,
            ICategoryService categoryService,
            IManufacturerService manufacturerService,
            IAclService aclService,
             IStoreMappingService storeMappingService,
             IStoreService storeService,
              IDiscountService discountService,
               IBackInStockSubscriptionService backInStockSubscriptionService)
        {
            this._captchaSettings = captchaSettings;
            this._customerService = customerService;
            this._downloadService = downloadService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            this._urlRecordService = urlRecordService;
            this._vendorAttributeParser = vendorAttributeParser;
            this._vendorAttributeService = vendorAttributeService;
            this._vendorModelFactory = vendorModelFactory;
            this._vendorService = vendorService;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._vendorSettings = vendorSettings;
            this._permissionService = permissionService;
            this._addressService = addressService;
            this._customerActivityService = customerActivityService;
            this._localizedEntityService = localizedEntityService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._productTagService = productTagService;
            this._shippingService = shippingService;
            this._categoryService = categoryService;
            this._manufacturerService = manufacturerService;
            this._aclService = aclService;
            this._storeMappingService = storeMappingService;
            this._storeService = storeService;
            this._discountService = discountService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdatePictureSeoNames(Vendor vendor)
        {
            var picture = _pictureService.GetPictureById(vendor.PictureId);
            if (picture != null)
                _pictureService.SetSeoFilename(picture.Id, _pictureService.GetPictureSeName(vendor.Name));
        }

        protected virtual string ParseVendorAttributes(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var attributesXml = "";
            var attributes = _vendorAttributeService.GetAllVendorAttributes();
            foreach (var attribute in attributes)
            {
                var controlId = $"vendor_attribute_{attribute.Id}";
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _vendorAttributeParser.AddVendorAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                )
                                {
                                    var selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _vendorAttributeParser.AddVendorAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //load read-only (already server-side selected) values
                            var attributeValues = _vendorAttributeService.GetVendorAttributeValues(attribute.Id);
                            foreach (var selectedAttributeId in attributeValues
                                .Where(v => v.IsPreSelected)
                                .Select(v => v.Id)
                                .ToList())
                            {
                                attributesXml = _vendorAttributeParser.AddVendorAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var enteredText = ctrlAttributes.ToString().Trim();
                                attributesXml = _vendorAttributeParser.AddVendorAttribute(attributesXml,
                                    attribute, enteredText);
                            }
                        }
                        break;
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.FileUpload:
                    //not supported vendor attributes
                    default:
                        break;
                }
            }

            return attributesXml;
        }

        #endregion

        #region Methods

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult ApplyVendor()
        {
            if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
                return RedirectToRoute("HomePage");

            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            var model = new ApplyVendorModel();
            model = _vendorModelFactory.PrepareApplyVendorModel(model, true, false, null);
            return View(model);
        }

        [HttpPost, ActionName("ApplyVendor")]
        [PublicAntiForgery]
        [ValidateCaptcha]
        public virtual IActionResult ApplyVendorSubmit(ApplyVendorModel model, bool captchaValid, IFormFile uploadedFile)
        {
            if (!_vendorSettings.AllowCustomersToApplyForVendorAccount)
                return RedirectToRoute("HomePage");

            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnApplyVendorPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            var pictureId = 0;

            if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
            {
                try
                {
                    var contentType = uploadedFile.ContentType;
                    var vendorPictureBinary = _downloadService.GetDownloadBits(uploadedFile);
                    var picture = _pictureService.InsertPicture(vendorPictureBinary, contentType, null);

                    if (picture != null)
                        pictureId = picture.Id;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", _localizationService.GetResource("Vendors.ApplyAccount.Picture.ErrorMessage"));
                }
            }

            //vendor attributes
            var vendorAttributesXml = ParseVendorAttributes(model.Form);
            _vendorAttributeParser.GetAttributeWarnings(vendorAttributesXml).ToList()
                .ForEach(warning => ModelState.AddModelError(string.Empty, warning));

            if (ModelState.IsValid)
            {
                var description = Core.Html.HtmlHelper.FormatText(model.Description, false, false, true, false, false, false);
                //disabled by default
                var vendor = new Vendor
                {
                    Name = model.Name,
                    Email = model.Email,
                    //some default settings
                    PageSize = 6,
                    AllowCustomersToSelectPageSize = true,
                    PageSizeOptions = _vendorSettings.DefaultVendorPageSizeOptions,
                    PictureId = pictureId,
                    Description = description
                };
                _vendorService.InsertVendor(vendor);
                //search engine name (the same as vendor name)
                var seName = _urlRecordService.ValidateSeName(vendor, vendor.Name, vendor.Name, true);
                _urlRecordService.SaveSlug(vendor, seName, 0);

                //associate to the current customer
                //but a store owner will have to manually add this customer role to "Vendors" role
                //if he wants to grant access to admin area
                _workContext.CurrentCustomer.VendorId = vendor.Id;
                _customerService.UpdateCustomer(_workContext.CurrentCustomer);

                //update picture seo file name
                UpdatePictureSeoNames(vendor);

                //save vendor attributes
                _genericAttributeService.SaveAttribute(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

                //notify store owner here (email)
                _workflowMessageService.SendNewVendorAccountApplyStoreOwnerNotification(_workContext.CurrentCustomer,
                    vendor, _localizationSettings.DefaultAdminLanguageId);

                model.DisableFormInput = true;
                model.Result = _localizationService.GetResource("Vendors.ApplyAccount.Submitted");
                return View(model);
            }

            //If we got this far, something failed, redisplay form
            model = _vendorModelFactory.PrepareApplyVendorModel(model, false, true, vendorAttributesXml);
            return View(model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Info()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            if (_workContext.CurrentVendor == null || !_vendorSettings.AllowVendorsToEditInfo)
                return RedirectToRoute("CustomerInfo");

            var model = new VendorInfoModel();
            model = _vendorModelFactory.PrepareVendorInfoModel(model, false);
            return View(model);
        }

        [HttpPost, ActionName("Info")]
        [PublicAntiForgery]
        [FormValueRequired("save-info-button")]
        public virtual IActionResult Info(VendorInfoModel model, IFormFile uploadedFile)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            if (_workContext.CurrentVendor == null || !_vendorSettings.AllowVendorsToEditInfo)
                return RedirectToRoute("CustomerInfo");

            Picture picture = null;

            if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
            {
                try
                {
                    var contentType = uploadedFile.ContentType;
                    var vendorPictureBinary = _downloadService.GetDownloadBits(uploadedFile);
                    picture = _pictureService.InsertPicture(vendorPictureBinary, contentType, null);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", _localizationService.GetResource("Account.VendorInfo.Picture.ErrorMessage"));
                }
            }

            var vendor = _workContext.CurrentVendor;
            var prevPicture = _pictureService.GetPictureById(vendor.PictureId);

            //vendor attributes
            var vendorAttributesXml = ParseVendorAttributes(model.Form);
            _vendorAttributeParser.GetAttributeWarnings(vendorAttributesXml).ToList()
                .ForEach(warning => ModelState.AddModelError(string.Empty, warning));

            if (ModelState.IsValid)
            {
                var description = Core.Html.HtmlHelper.FormatText(model.Description, false, false, true, false, false, false);

                vendor.Name = model.Name;
                vendor.Email = model.Email;
                vendor.Description = description;

                if (picture != null)
                {
                    vendor.PictureId = picture.Id;

                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }

                //update picture seo file name
                UpdatePictureSeoNames(vendor);

                _vendorService.UpdateVendor(vendor);

                //save vendor attributes
                _genericAttributeService.SaveAttribute(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

                //notifications
                if (_vendorSettings.NotifyStoreOwnerAboutVendorInformationChange)
                    _workflowMessageService.SendVendorInformationChangeNotification(vendor, _localizationSettings.DefaultAdminLanguageId);

                return RedirectToAction("Info");
            }

            //If we got this far, something failed, redisplay form
            model = _vendorModelFactory.PrepareVendorInfoModel(model, true, vendorAttributesXml);
            return View(model);
        }

        [HttpPost, ActionName("Info")]
        [PublicAntiForgery]
        [FormValueRequired("remove-picture")]
        public virtual IActionResult RemovePicture()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            if (_workContext.CurrentVendor == null || !_vendorSettings.AllowVendorsToEditInfo)
                return RedirectToRoute("CustomerInfo");

            var vendor = _workContext.CurrentVendor;
            var picture = _pictureService.GetPictureById(vendor.PictureId);

            if (picture != null)
                _pictureService.DeletePicture(picture);

            vendor.PictureId = 0;
            _vendorService.UpdateVendor(vendor);

            //notifications
            if (_vendorSettings.NotifyStoreOwnerAboutVendorInformationChange)
                _workflowMessageService.SendVendorInformationChangeNotification(vendor, _localizationSettings.DefaultAdminLanguageId);

            return RedirectToAction("Info");
        }

        [HttpPost, ActionName("customerquestion")]
        [PublicAntiForgery]
        public virtual IActionResult CustomerQuestion(string customerQuestion, int vendorId, string vendorSeName)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            var customerId = _workContext.CurrentCustomer.Id;

            this._vendorService.SaveVendorStories(vendorId, customerId, customerQuestion, false);

            return RedirectToRoute("Vendor", new { SeName = vendorSeName });
        }

        [HttpPost, ActionName("chats")]
        public virtual IActionResult Chats(int vendorId, int customerId)
        {
            var chat = this._vendorService.GetVendorChat(vendorId, customerId);

            return Json(new { Chat = chat });
        }

        [HttpPost, ActionName("followme")]
        [PublicAntiForgery]
        public virtual IActionResult FollowMe(int vendorId, string vendorSeName)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Challenge();

            var customerId = _workContext.CurrentCustomer.Id;

            this._vendorService.SaveFollower(vendorId, customerId);

            return RedirectToRoute("Vendor", new { SeName = vendorSeName });
        }

        [HttpGet]
        public virtual IActionResult BestVendorList(VendorPagingFilteringModel command)
        {
            var vendors = _vendorService.GetAllBestVendors("", pageIndex: command.PageNumber > 0 ? command.PageNumber - 1 : command.PageNumber,
                pageSize: 25);

            //prepare model
            var model = _vendorModelFactory.PrepareVendorListModel(vendors, command);
            model.VendorType = Enums.VendorTypeEnum.BestReviewed;

            return View("VendorList", model);
        }

        [HttpGet]
        public virtual IActionResult MostPopularVendorList(VendorPagingFilteringModel command)
        {
            var vendors = _vendorService.GetAllMostPopularVendors("", pageIndex: command.PageNumber > 0 ? command.PageNumber - 1 : command.PageNumber,
                pageSize: 25);

            //prepare model
            var model = _vendorModelFactory.PrepareVendorListModel(vendors, command);
            model.VendorType = Enums.VendorTypeEnum.MostPopular;

            return View("VendorList", model);
        }

        #endregion

        #region VendorForm
        public virtual IActionResult Edit(string email)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
            //    return AccessDeniedView();

            var vendor = _vendorService.GetVendorByEmail(email);
            if (vendor == null || vendor.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = _vendorModelFactory.PrepareVendorModel(null, vendor);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(VendorEditModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                return AccessDeniedView();

            //try to get a vendor with the specified id
            var vendor = _vendorService.GetVendorById(model.Id);
            if (vendor == null || vendor.Deleted)
                return RedirectToAction("List");

            //parse vendor attributes
            var vendorAttributesXml = ParseVendorAttributes(model.Form);
            _vendorAttributeParser.GetAttributeWarnings(vendorAttributesXml).ToList()
                .ForEach(warning => ModelState.AddModelError(string.Empty, warning));

            if (ModelState.IsValid)
            {
                var prevPictureId = vendor.PictureId;
                //vendor = model.ToEntity(vendor);

                vendor.Active = model.Active;
                vendor.BirthDate = model.BirthDate;
                vendor.City = model.City;
                vendor.CountryId = model.CountryId;
                vendor.Description = model.Description;
                vendor.Email = model.Email;
                vendor.FollowersNumber = model.FollowersNumber;
                vendor.Id = model.Id;
                vendor.Name = model.Name;
                vendor.PictureId = model.PictureId;
                vendor.ShopName = model.ShopName;

                _vendorService.UpdateVendor(vendor);

                //vendor attributes
                _genericAttributeService.SaveAttribute(vendor, NopVendorDefaults.VendorAttributes, vendorAttributesXml);

                //activity log
                _customerActivityService.InsertActivity("EditVendor",
                    string.Format(_localizationService.GetResource("ActivityLog.EditVendor"), vendor.Id), vendor);

                //search engine name
                if (string.IsNullOrEmpty(vendor.ShopName))
                {
                    model.SeName = _urlRecordService.ValidateSeName(vendor, model.SeName, vendor.Name, true);
                }
                else
                {
                    model.SeName = _urlRecordService.ValidateSeName(vendor, model.ShopName, vendor.ShopName, true);
                }
                _urlRecordService.SaveSlug(vendor, model.SeName, 0);

                //address
                var address = _addressService.GetAddressById(vendor.AddressId);
                if (address == null)
                {
                    address = model.Address.ToEntity<Address>();
                    address.CreatedOnUtc = DateTime.UtcNow;

                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;
                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;

                    _addressService.InsertAddress(address);
                    vendor.AddressId = address.Id;
                    _vendorService.UpdateVendor(vendor);
                }
                else
                {
                    address = model.Address.ToEntity(address);

                    //some validation
                    if (address.CountryId == 0)
                        address.CountryId = null;
                    if (address.StateProvinceId == 0)
                        address.StateProvinceId = null;

                    _addressService.UpdateAddress(address);
                }

                //locales
                UpdateLocales(vendor, model);

                //delete an old picture (if deleted or updated)
                if (prevPictureId > 0 && prevPictureId != vendor.PictureId)
                {
                    var prevPicture = _pictureService.GetPictureById(prevPictureId);
                    if (prevPicture != null)
                        _pictureService.DeletePicture(prevPicture);
                }
                //update picture seo file name
                UpdatePictureSeoNames(vendor);

                SuccessNotification(_localizationService.GetResource("Admin.Vendors.Updated"));

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = vendor.Id });
            }

            //prepare model
            model = _vendorModelFactory.PrepareVendorModel(model, vendor, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        protected virtual void UpdateLocales(Vendor vendor, VendorEditModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.Description,
                    localized.Description,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(vendor,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(vendor, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(vendor, seName, localized.LanguageId);
            }
        }

        #endregion

        #region VendorProduct

        [HttpPost]
        public virtual IActionResult VendorProductList(ProductSearchModel searchModel)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedKendoGridJson();

            //prepare model
            var model = _productModelFactory.PrepareProductListModel(searchModel);

            return Json(model);
        }


        public virtual IActionResult VendorProduct(int id)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            //try to get a product with the specified id
            var product = _productService.GetProductById(id);
            if (product == null || product.Deleted)
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            //prepare model
            var model = _productModelFactory.PrepareProductModel(null, product);

            //return View(model);
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult VendorProduct(ProductModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            //try to get a product with the specified id
            var product = _productService.GetProductById(model.Id);
            if (product == null || product.Deleted)
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            //check if the product quantity has been changed while we were editing the product
            //and if it has been changed then we show error notification
            //and redirect on the editing page without data saving
            if (product.StockQuantity != model.LastStockQuantity)
            {
                ErrorNotification(_localizationService.GetResource("Admin.Catalog.Products.Fields.StockQuantity.ChangedWarning"));
                return RedirectToAction("Edit", new { id = product.Id });
            }

            if (ModelState.IsValid)
            {
                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                    model.VendorId = _workContext.CurrentVendor.Id;

                //we do not validate maximum number of products per vendor when editing existing products (only during creation of new products)
                //vendors cannot edit "Show on home page" property
                if (_workContext.CurrentVendor != null && model.ShowOnHomePage != product.ShowOnHomePage)
                    model.ShowOnHomePage = product.ShowOnHomePage;

                //some previously used values
                var prevTotalStockQuantity = _productService.GetTotalStockQuantity(product);
                var prevDownloadId = product.DownloadId;
                var prevSampleDownloadId = product.SampleDownloadId;
                var previousStockQuantity = product.StockQuantity;
                var previousWarehouseId = product.WarehouseId;

                //product
                var amountSold = product.AmountSold;
                product = model.ToEntity(product);
                product.AmountSold = amountSold;
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.UpdateProduct(product);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(product, model.SeName, product.Name, true);
                _urlRecordService.SaveSlug(product, model.SeName, 0);

                //locales
                UpdateLocales(product, model);

                //tags
                _productTagService.UpdateProductTags(product, ParseProductTags(model.ProductTags));

                //warehouses
                SaveProductWarehouseInventory(product, model);

                //categories
                SaveCategoryMappings(product, model);

                //manufacturers
                SaveManufacturerMappings(product, model);

                //ACL (customer roles)
                SaveProductAcl(product, model);

                //stores
                SaveStoreMappings(product, model);

                //discounts
                SaveDiscountMappings(product, model);

                //picture seo names
                UpdatePictureSeoNames(product);

                //back in stock notifications
                if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                    product.BackorderMode == BackorderMode.NoBackorders &&
                    product.AllowBackInStockSubscriptions &&
                    _productService.GetTotalStockQuantity(product) > 0 &&
                    prevTotalStockQuantity <= 0 &&
                    product.Published &&
                    !product.Deleted)
                {
                    _backInStockSubscriptionService.SendNotificationsToSubscribers(product);
                }

                //delete an old "download" file (if deleted or updated)
                if (prevDownloadId > 0 && prevDownloadId != product.DownloadId)
                {
                    var prevDownload = _downloadService.GetDownloadById(prevDownloadId);
                    if (prevDownload != null)
                        _downloadService.DeleteDownload(prevDownload);
                }

                //delete an old "sample download" file (if deleted or updated)
                if (prevSampleDownloadId > 0 && prevSampleDownloadId != product.SampleDownloadId)
                {
                    var prevSampleDownload = _downloadService.GetDownloadById(prevSampleDownloadId);
                    if (prevSampleDownload != null)
                        _downloadService.DeleteDownload(prevSampleDownload);
                }

                //quantity change history
                if (previousWarehouseId != product.WarehouseId)
                {
                    //warehouse is changed 
                    //compose a message
                    var oldWarehouseMessage = string.Empty;
                    if (previousWarehouseId > 0)
                    {
                        var oldWarehouse = _shippingService.GetWarehouseById(previousWarehouseId);
                        if (oldWarehouse != null)
                            oldWarehouseMessage = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.EditWarehouse.Old"), oldWarehouse.Name);
                    }

                    var newWarehouseMessage = string.Empty;
                    if (product.WarehouseId > 0)
                    {
                        var newWarehouse = _shippingService.GetWarehouseById(product.WarehouseId);
                        if (newWarehouse != null)
                            newWarehouseMessage = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.EditWarehouse.New"), newWarehouse.Name);
                    }

                    var message = string.Format(_localizationService.GetResource("Admin.StockQuantityHistory.Messages.EditWarehouse"), oldWarehouseMessage, newWarehouseMessage);

                    //record history
                    _productService.AddStockQuantityHistoryEntry(product, -previousStockQuantity, 0, previousWarehouseId, message);
                    _productService.AddStockQuantityHistoryEntry(product, product.StockQuantity, product.StockQuantity, product.WarehouseId, message);
                }
                else
                {
                    _productService.AddStockQuantityHistoryEntry(product, product.StockQuantity - previousStockQuantity, product.StockQuantity,
                        product.WarehouseId, _localizationService.GetResource("Admin.StockQuantityHistory.Messages.Edit"));
                }

                //activity log
                _customerActivityService.InsertActivity("EditProduct",
                    string.Format(_localizationService.GetResource("ActivityLog.EditProduct"), product.Name), product);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Updated"));

                //if (!continueEditing)
                //    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                var vendor = _vendorService.GetVendorById(product.VendorId);

                return RedirectToAction("Edit", "Vendor", new { email = vendor.Email });
            }

            //prepare model
            model = _productModelFactory.PrepareProductModel(model, product, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        protected virtual void UpdatePictureSeoNames(Product product)
        {
            foreach (var pp in product.ProductPictures)
                _pictureService.SetSeoFilename(pp.PictureId, _pictureService.GetPictureSeName(product.Name));
        }

        protected virtual void SaveDiscountMappings(Product product, ProductModel model)
        {
            var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToSkus, showHidden: true);

            foreach (var discount in allDiscounts)
            {
                if (model.SelectedDiscountIds != null && model.SelectedDiscountIds.Contains(discount.Id))
                {
                    //new discount
                    if (product.DiscountProductMappings.Count(mapping => mapping.DiscountId == discount.Id) == 0)
                        product.DiscountProductMappings.Add(new DiscountProductMapping { Discount = discount });
                }
                else
                {
                    //remove discount
                    if (product.DiscountProductMappings.Count(mapping => mapping.DiscountId == discount.Id) > 0)
                    {
                        product.DiscountProductMappings
                            .Remove(product.DiscountProductMappings.FirstOrDefault(mapping => mapping.DiscountId == discount.Id));
                    }
                }
            }

            _productService.UpdateProduct(product);
            _productService.UpdateHasDiscountsApplied(product);
        }


        protected virtual void SaveStoreMappings(Product product, ProductModel model)
        {
            product.LimitedToStores = model.SelectedStoreIds.Any();

            var existingStoreMappings = _storeMappingService.GetStoreMappings(product);
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                if (model.SelectedStoreIds.Contains(store.Id))
                {
                    //new store
                    if (existingStoreMappings.Count(sm => sm.StoreId == store.Id) == 0)
                        _storeMappingService.InsertStoreMapping(product, store.Id);
                }
                else
                {
                    //remove store
                    var storeMappingToDelete = existingStoreMappings.FirstOrDefault(sm => sm.StoreId == store.Id);
                    if (storeMappingToDelete != null)
                        _storeMappingService.DeleteStoreMapping(storeMappingToDelete);
                }
            }
        }

        protected virtual void SaveProductAcl(Product product, ProductModel model)
        {
            product.SubjectToAcl = model.SelectedCustomerRoleIds.Any();

            var existingAclRecords = _aclService.GetAclRecords(product);
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            foreach (var customerRole in allCustomerRoles)
            {
                if (model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                {
                    //new role
                    if (existingAclRecords.Count(acl => acl.CustomerRoleId == customerRole.Id) == 0)
                        _aclService.InsertAclRecord(product, customerRole.Id);
                }
                else
                {
                    //remove role
                    var aclRecordToDelete = existingAclRecords.FirstOrDefault(acl => acl.CustomerRoleId == customerRole.Id);
                    if (aclRecordToDelete != null)
                        _aclService.DeleteAclRecord(aclRecordToDelete);
                }
            }
        }

        protected virtual void SaveManufacturerMappings(Product product, ProductModel model)
        {
            var existingProductManufacturers = _manufacturerService.GetProductManufacturersByProductId(product.Id, true);

            //delete manufacturers
            foreach (var existingProductManufacturer in existingProductManufacturers)
                if (!model.SelectedManufacturerIds.Contains(existingProductManufacturer.ManufacturerId))
                    _manufacturerService.DeleteProductManufacturer(existingProductManufacturer);

            //add manufacturers
            foreach (var manufacturerId in model.SelectedManufacturerIds)
            {
                if (_manufacturerService.FindProductManufacturer(existingProductManufacturers, product.Id, manufacturerId) == null)
                {
                    //find next display order
                    var displayOrder = 1;
                    var existingManufacturerMapping = _manufacturerService.GetProductManufacturersByManufacturerId(manufacturerId, showHidden: true);
                    if (existingManufacturerMapping.Any())
                        displayOrder = existingManufacturerMapping.Max(x => x.DisplayOrder) + 1;
                    _manufacturerService.InsertProductManufacturer(new ProductManufacturer
                    {
                        ProductId = product.Id,
                        ManufacturerId = manufacturerId,
                        DisplayOrder = displayOrder
                    });
                }
            }
        }

        protected virtual void SaveCategoryMappings(Product product, ProductModel model)
        {
            var existingProductCategories = _categoryService.GetProductCategoriesByProductId(product.Id, true);

            //delete categories
            foreach (var existingProductCategory in existingProductCategories)
                if (!model.SelectedCategoryIds.Contains(existingProductCategory.CategoryId))
                    _categoryService.DeleteProductCategory(existingProductCategory);

            //add categories
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                if (_categoryService.FindProductCategory(existingProductCategories, product.Id, categoryId) == null)
                {
                    //find next display order
                    var displayOrder = 1;
                    var existingCategoryMapping = _categoryService.GetProductCategoriesByCategoryId(categoryId, showHidden: true);
                    if (existingCategoryMapping.Any())
                        displayOrder = existingCategoryMapping.Max(x => x.DisplayOrder) + 1;
                    _categoryService.InsertProductCategory(new ProductCategory
                    {
                        ProductId = product.Id,
                        CategoryId = categoryId,
                        DisplayOrder = displayOrder
                    });
                }
            }
        }

        protected virtual void SaveProductWarehouseInventory(Product product, ProductModel model)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (model.ManageInventoryMethodId != (int)ManageInventoryMethod.ManageStock)
                return;

            if (!model.UseMultipleWarehouses)
                return;

            var warehouses = _shippingService.GetAllWarehouses();

            var formData = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());

            foreach (var warehouse in warehouses)
            {
                //parse stock quantity
                var stockQuantity = 0;
                foreach (var formKey in formData.Keys)
                {
                    if (!formKey.Equals($"warehouse_qty_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    int.TryParse(formData[formKey], out stockQuantity);
                    break;
                }

                //parse reserved quantity
                var reservedQuantity = 0;
                foreach (var formKey in formData.Keys)
                    if (formKey.Equals($"warehouse_reserved_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int.TryParse(formData[formKey], out reservedQuantity);
                        break;
                    }

                //parse "used" field
                var used = false;
                foreach (var formKey in formData.Keys)
                    if (formKey.Equals($"warehouse_used_{warehouse.Id}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int.TryParse(formData[formKey], out var tmp);
                        used = tmp == warehouse.Id;
                        break;
                    }

                //quantity change history message
                var message = $"{_localizationService.GetResource("Admin.StockQuantityHistory.Messages.MultipleWarehouses")} {_localizationService.GetResource("Admin.StockQuantityHistory.Messages.Edit")}";

                var existingPwI = product.ProductWarehouseInventory.FirstOrDefault(x => x.WarehouseId == warehouse.Id);
                if (existingPwI != null)
                {
                    if (used)
                    {
                        var previousStockQuantity = existingPwI.StockQuantity;

                        //update existing record
                        existingPwI.StockQuantity = stockQuantity;
                        existingPwI.ReservedQuantity = reservedQuantity;
                        _productService.UpdateProduct(product);

                        //quantity change history
                        _productService.AddStockQuantityHistoryEntry(product, existingPwI.StockQuantity - previousStockQuantity, existingPwI.StockQuantity,
                            existingPwI.WarehouseId, message);
                    }
                    else
                    {
                        //delete. no need to store record for qty 0
                        _productService.DeleteProductWarehouseInventory(existingPwI);

                        //quantity change history
                        _productService.AddStockQuantityHistoryEntry(product, -existingPwI.StockQuantity, 0, existingPwI.WarehouseId, message);
                    }
                }
                else
                {
                    if (!used)
                        continue;

                    //no need to insert a record for qty 0
                    existingPwI = new ProductWarehouseInventory
                    {
                        WarehouseId = warehouse.Id,
                        ProductId = product.Id,
                        StockQuantity = stockQuantity,
                        ReservedQuantity = reservedQuantity
                    };
                    product.ProductWarehouseInventory.Add(existingPwI);
                    _productService.UpdateProduct(product);

                    //quantity change history
                    _productService.AddStockQuantityHistoryEntry(product, existingPwI.StockQuantity, existingPwI.StockQuantity,
                        existingPwI.WarehouseId, message);
                }
            }
        }

        protected virtual string[] ParseProductTags(string productTags)
        {
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(productTags))
                return result.ToArray();

            var values = productTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var val in values)
                if (!string.IsNullOrEmpty(val.Trim()))
                    result.Add(val.Trim());

            return result.ToArray();
        }

        protected virtual void UpdateLocales(Product product, ProductModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.ShortDescription,
                    localized.ShortDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.FullDescription,
                    localized.FullDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.MetaKeywords,
                    localized.MetaKeywords,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.MetaDescription,
                    localized.MetaDescription,
                    localized.LanguageId);
                _localizedEntityService.SaveLocalizedValue(product,
                    x => x.MetaTitle,
                    localized.MetaTitle,
                    localized.LanguageId);

                //search engine name
                var seName = _urlRecordService.ValidateSeName(product, localized.SeName, localized.Name, false);
                _urlRecordService.SaveSlug(product, seName, localized.LanguageId);
            }
        }

        public virtual IActionResult CreateVendorProduct()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && _workContext.CurrentVendor != null
                && _productService.GetNumberOfProductsByVendorId(_workContext.CurrentVendor.Id) >= _vendorSettings.MaximumProductNumber)
            {
                ErrorNotification(string.Format(_localizationService.GetResource("Admin.Catalog.Products.ExceededMaximumNumber"),
                    _vendorSettings.MaximumProductNumber));
                return RedirectToAction("List");
            }

            //prepare model
            var model = _productModelFactory.PrepareProductModel(new ProductModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult CreateVendorProduct(ProductModel model, bool continueEditing)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            //validate maximum number of products per vendor
            if (_vendorSettings.MaximumProductNumber > 0 && _workContext.CurrentVendor != null
                && _productService.GetNumberOfProductsByVendorId(_workContext.CurrentVendor.Id) >= _vendorSettings.MaximumProductNumber)
            {
                ErrorNotification(string.Format(_localizationService.GetResource("Admin.Catalog.Products.ExceededMaximumNumber"),
                    _vendorSettings.MaximumProductNumber));
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {
                //a vendor should have access only to his products
                if (_workContext.CurrentVendor != null)
                    model.VendorId = _workContext.CurrentVendor.Id;

                //vendors cannot edit "Show on home page" property
                if (_workContext.CurrentVendor != null && model.ShowOnHomePage)
                    model.ShowOnHomePage = false;

                //product
                var product = model.ToEntity<Product>();
                product.CreatedOnUtc = DateTime.UtcNow;
                product.UpdatedOnUtc = DateTime.UtcNow;
                _productService.InsertProduct(product);

                //search engine name
                model.SeName = _urlRecordService.ValidateSeName(product, model.SeName, product.Name, true);
                _urlRecordService.SaveSlug(product, model.SeName, 0);

                //locales
                UpdateLocales(product, model);

                //categories
                SaveCategoryMappings(product, model);

                //manufacturers
                SaveManufacturerMappings(product, model);

                //ACL (customer roles)
                SaveProductAcl(product, model);

                //stores
                SaveStoreMappings(product, model);

                //discounts
                SaveDiscountMappings(product, model);

                //tags
                _productTagService.UpdateProductTags(product, ParseProductTags(model.ProductTags));

                //warehouses
                SaveProductWarehouseInventory(product, model);

                //quantity change history
                _productService.AddStockQuantityHistoryEntry(product, product.StockQuantity, product.StockQuantity, product.WarehouseId,
                    _localizationService.GetResource("Admin.StockQuantityHistory.Messages.Edit"));

                //activity log
                _customerActivityService.InsertActivity("AddNewProduct",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewProduct"), product.Name), product);

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Added"));

                //if (!continueEditing)
                //    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                var vendor = _vendorService.GetVendorById(product.VendorId);

                //return RedirectToAction("Edit", new { id = product.Id });

                return RedirectToAction("Edit", "Vendor", new { email = vendor.Email });
            }

            //prepare model
            model = _productModelFactory.PrepareProductModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //try to get a product with the specified id
            var product = _productService.GetProductById(id);
            if (product == null)
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            _productService.DeleteProduct(product);

            //activity log
            _customerActivityService.InsertActivity("DeleteProduct",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteProduct"), product.Name), product);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.Products.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            //    return AccessDeniedView();

            if (selectedIds != null)
            {
                _productService.DeleteProducts(_productService.GetProductsByIds(selectedIds.ToArray()).Where(p => _workContext.CurrentVendor == null || p.VendorId == _workContext.CurrentVendor.Id).ToList());
            }

            return Json(new { Result = true });
        }


        #endregion

    }
}