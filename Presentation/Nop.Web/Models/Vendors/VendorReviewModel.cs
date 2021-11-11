using System;

namespace Nop.Web.Models.Vendors
{
    public class VendorReviewModel
    {
        public int Rating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCountry { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}