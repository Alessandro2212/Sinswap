using Nop.Web.Framework.Models;

namespace Nop.Web.Models.Customer
{
    /// <summary>
    /// Represents a reward points search model
    /// </summary>
    public partial class CustomerRewardPointsSearchModel : BaseSearchModel
    {
        #region Properties

        public int CustomerId { get; set; }
        
        #endregion
    }
}