using Nop.Web.Framework.Models;
using System.Collections.Generic;

namespace Nop.Web.Models.Vendors
{
    public class PremiumVendorSmallTalkModel : BaseNopModel
    {
        public IEnumerable<VendorQuestionModel> VendorQuestions { get; set; }
    }
}