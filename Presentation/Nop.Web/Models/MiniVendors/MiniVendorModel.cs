using FluentValidation.Attributes;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Models.MiniVendors
{
    [Validator(typeof(MiniVendorModel))]
    public class MiniVendorModel : BaseNopModel
    {
        public MiniVendorModel()
        {
        }

        public int Id { get; set; }

        [NopResourceDisplayName("Account.VendorInfo.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Account.VendorInfo.Age")]
        public int Age { get; set; }

        [NopResourceDisplayName("Account.VendorInfo.City")]
        public string City { get; set; }

        public string Country { get; set; }

        public int? FollowersNumber { get; set; }

        [NopResourceDisplayName("Account.VendorInfo.Picture")]
        public string PictureUrl { get; set; }

        public string SeName { get; set; }

    }
}