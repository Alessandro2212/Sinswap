using Nop.Web.Models.Common;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial interface IFaqModelFactory
    {
        FaqViewModel PrepareFrequentFaqViewModel(IEnumerable<string> strings);
        FaqViewModel PrepareFaqViewModel();
    }
}