﻿using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer
{
    /// <summary>
    /// Represents a customer back in stock subscriptions search model
    /// </summary>
    public partial class CustomerBackInStockSubscriptionSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }

        #endregion
    }
}