using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Catalog
{
    public partial class SearchModel : BaseNopModel
    {
        public SearchModel()
        {
            this.PagingFilteringContext = new CatalogPagingFilteringModel();
            this.Categories = new List<Catalog.CategoryModel>();
            this.Vendors = new List<VendorModel>();
            this.AvailableCategories = new List<SelectListItem>();
            this.AvailableManufacturers = new List<SelectListItem>();
            this.AvailableVendors = new List<SelectListItem>();
            this.ProductPriceFromOptions = new Dictionary<decimal, string>
                                            {
                                                { decimal.MinValue,"Min. price" },
                                                { 5,"€ 5" },
                                                { 10,"€ 10 " },
                                                { 50,"€ 50" },
                                                { 100,"€ 100" },
                                            };

            this.ProductPriceToOptions = new Dictionary<decimal, string>
                                            {
                                                { 200,"€ 200" },
                                                { 500,"€ 500" },
                                                { 1000,"€ 1000" },
                                                { 2000,"€ 20000" },
                                                { decimal.MaxValue,"No max. price" },
                                            };

        }

        public string Warning { get; set; }

        public bool NoResults { get; set; }

        /// <summary>
        /// Query string
        /// </summary>
        [NopResourceDisplayName("Search.SearchTerm")]
        public string q { get; set; }

        /// <summary>
        /// Category ID
        /// </summary>
        [NopResourceDisplayName("Search.Category")]
        public int cid { get; set; }

        [NopResourceDisplayName("Search.IncludeSubCategories")]
        public bool isc { get; set; }

        /// <summary>
        /// Manufacturer ID
        /// </summary>
        [NopResourceDisplayName("Search.Manufacturer")]
        public int mid { get; set; }

        /// <summary>
        /// Vendor ID
        /// </summary>
        [NopResourceDisplayName("Search.Vendor")]
        public int vid { get; set; }

        /// <summary>
        /// Price - From 
        /// </summary>
        public string pf { get; set; }

        public decimal ProductPriceFrom { get; set; } = decimal.MinValue;
        public Dictionary<decimal, string> ProductPriceFromOptions { get; set; }

        /// <summary>
        /// Price - To
        /// </summary>
        public string pt { get; set; }

        public decimal ProductPriceTo { get; set; } = decimal.MaxValue;
        public Dictionary<decimal, string> ProductPriceToOptions { get; set; }

        /// <summary>
        /// A value indicating whether to search in descriptions
        /// </summary>
        [NopResourceDisplayName("Search.SearchInDescriptions")]
        public bool sid { get; set; }

        /// <summary>
        /// A value indicating whether "advanced search" is enabled
        /// </summary>
        [NopResourceDisplayName("Search.AdvancedSearch")]
        public bool adv { get; set; }

        /// <summary>
        /// A value indicating whether "allow search by vendor" is enabled
        /// </summary>
        public bool asv { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableManufacturers { get; set; }
        public IList<SelectListItem> AvailableVendors { get; set; }


        public CatalogPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<Catalog.CategoryModel> Categories { get; set; }
        //public IList<ProductOverviewModel> Products { get; set; }
        public IList<VendorModel> Vendors { get; set; }
        //public IList<SearchElementModel> SearchElementModels { get; set; }

        #region Nested classes

        public class CategoryModel : BaseNopEntityModel
        {
            public string Breadcrumb { get; set; }
        }

        #endregion
    }
}