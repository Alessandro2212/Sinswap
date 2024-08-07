﻿using System.Collections.Generic;
using Nop.Web.Framework.Models;
using Nop.Web.Models.Media;

namespace Nop.Web.Models.Catalog
{
    public class CategorySimpleModel : BaseNopEntityModel
    {
        public CategorySimpleModel()
        {
            SubCategories = new List<CategorySimpleModel>();
        }

        public string Name { get; set; }

        public string SeName { get; set; }

        public int? NumberOfProducts { get; set; }

        public bool IncludeInTopMenu { get; set; }

        public bool IsFavourite { get; set; }

        public int? SoldItems { get; set; }

        public int? TotalRatings { get; set; }

        public PictureModel PictureModel { get; set; }

        public string ParentCategoryName { get; set; }

        public List<CategorySimpleModel> SubCategories { get; set; }
    }
}