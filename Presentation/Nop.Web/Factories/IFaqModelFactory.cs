using Nop.Web.Models.Common;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial interface IFaqModelFactory
    {
        FaqViewModel PrepareFaqViewModel(IEnumerable<string> strings);
    }
}