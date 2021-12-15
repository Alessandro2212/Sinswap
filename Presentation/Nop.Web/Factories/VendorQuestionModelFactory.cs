using Nop.Services.Vendors;
using Nop.Web.Models.Vendors;
using System.Collections.Generic;

namespace Nop.Web.Factories
{
    public partial class VendorQuestionModelFactory : IVendorQuestionModelFactory
    {
        private readonly IVendorService _vendorService;

        public VendorQuestionModelFactory(IVendorService vendorService)
        {
            this._vendorService = vendorService;
        }

        public VendorFaqViewModel GetVendorFaqs(int vendorId, int amount)
        {
            //call the service
            var vendorFaqs = this._vendorService.GetVendorFaqs(vendorId, amount);

            //prepare model for the view
            VendorFaqViewModel vendorFaqModel = new VendorFaqViewModel();
            List<VendorFaqModel> vendorFaqModels = new List<VendorFaqModel>();
            foreach (var vf in vendorFaqs)
            {
                var vendorFaq = new VendorFaqModel();
                vendorFaq.Question = vf.QuestionText;
                vendorFaq.Answer = vf.ReplyText;
                vendorFaqModels.Add(vendorFaq);
            }

            vendorFaqModel.VendorFaqs = vendorFaqModels;

            return vendorFaqModel;
        }
    }
}