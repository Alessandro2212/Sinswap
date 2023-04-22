using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer
{
    /// <summary>
    /// Represents a customer orders search model
    /// </summary>
    public partial class CustomerOrderSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }

        #endregion
    }
}