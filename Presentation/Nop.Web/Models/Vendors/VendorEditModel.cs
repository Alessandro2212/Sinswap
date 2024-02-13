﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Models.Common;
using Nop.Web.Validators.Vendors;

namespace Nop.Web.Models.Vendors
{
    /// <summary>
    /// Represents a vendor model
    /// </summary>
    [Validator(typeof(VendorValidator))]
    public partial class VendorEditModel : BaseNopEntityModel, ILocalizedModel<VendorLocalizedModel>
    {
        #region Ctor

        public VendorEditModel()
        {
            if (PageSize < 1)
                PageSize = 5;

            Address = new AddressModel();
            VendorAttributes = new List<VendorAttributeModel>();
            Locales = new List<VendorLocalizedModel>();
            AssociatedCustomers = new List<VendorAssociatedCustomerModel>();
            VendorNoteSearchModel = new VendorNoteSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Vendors.Fields.Name")]

        public string Name { get; set; }

        [DisplayName("Shop Name")]
        public string ShopName { get; set; }

        [DataType(DataType.EmailAddress)]
        [NopResourceDisplayName("Admin.Vendors.Fields.Email")]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [UIHint("Picture")]
        [NopResourceDisplayName("Admin.Vendors.Fields.Picture")]
        public int PictureId { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.AdminComment")]
        public string AdminComment { get; set; }

        public Common.AddressModel Address { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.Active")]
        public bool Active { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.PageSize")]
        public int PageSize { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.AllowCustomersToSelectPageSize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [DisplayName("Birthdate")]
        public DateTime BirthDate { get; set; }

        [DisplayName("Is Premium")]
        public bool? IsPremium { get; set; }

        [DisplayName("Followers")]
        public int FollowersNumber { get; set; }

        public string City { get; set; }

        public int CountryId { get; set; }

        public bool IsError { get; set; }

        public List<VendorAttributeModel> VendorAttributes { get; set; }

        public IList<VendorLocalizedModel> Locales { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.AssociatedCustomerEmails")]
        public IList<VendorAssociatedCustomerModel> AssociatedCustomers { get; set; }

        //vendor notes
        [NopResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string AddVendorNoteMessage { get; set; }

        public VendorNoteSearchModel VendorNoteSearchModel { get; set; }

        //extra fields for vendor (and customer) (backend) form
        public string FavouriteHobby { get; set; }
        public string FavouriteMovie { get; set; }
        public string FavouriteWear { get; set; }
        public bool DoesPartnerKnow { get; set; }
        public string FavouriteThing { get; set; }
        public string FavouriteFood { get; set; }
        public string FavouriteKink { get; set; }
        public string Secrets { get; set; }
        public string Phone { get; set; }
        public string PictureUrl { get; set; } = "https://placehold.co/240x300";
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }


        #endregion

        #region Nested classes

        public partial class VendorAttributeModel : BaseNopEntityModel
        {
            public VendorAttributeModel()
            {
                Values = new List<VendorAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<VendorAttributeValueModel> Values { get; set; }
        }

        public partial class VendorAttributeValueModel : BaseNopEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }

    public partial class VendorLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [NopResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }
    }
}