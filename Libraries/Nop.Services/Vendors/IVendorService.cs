using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;

namespace Nop.Services.Vendors
{
    /// <summary>
    /// Vendor service interface
    /// </summary>
    public partial interface IVendorService
    {
        /// <summary>
        /// Gets a vendor by vendor identifier
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <returns>Vendor</returns>
        Vendor GetVendorById(int vendorId);
        Vendor GetVendorByEmail(string email);

        /// <summary>
        /// Delete a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        void DeleteVendor(Vendor vendor);

        /// <summary>
        /// Gets all vendors
        /// </summary>
        /// <param name="name">Vendor name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Vendors</returns>
        IPagedList<Vendor> GetAllVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        IPagedList<Vendor> GetAllMostPopularVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
        IPagedList<Vendor> GetAllBestVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<Vendor> GetAllVendorsForCategory(int categoryId, string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IEnumerable<Vendor> GetVendorsFromReviewsAndCustomer(int customerId);

        IEnumerable<Vendor> GetVendorsFromCustomerPurchasedItems(int customerId);
        int GetNumberOfPurchasedItems(int customerId, int vendorId);

        /// <summary>
        /// Gets vendors
        /// </summary>
        /// <param name="vendorIds">Vendor identifiers</param>
        /// <returns>Vendors</returns>
        IList<Vendor> GetVendorsByIds(int[] vendorIds);

        /// <summary>
        /// Inserts a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        void InsertVendor(Vendor vendor);

        /// <summary>
        /// Updates the vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        void UpdateVendor(Vendor vendor);

        /// <summary>
        /// Gets a vendor note
        /// </summary>
        /// <param name="vendorNoteId">The vendor note identifier</param>
        /// <returns>Vendor note</returns>
        VendorNote GetVendorNoteById(int vendorNoteId);

        /// <summary>
        /// Deletes a vendor note
        /// </summary>
        /// <param name="vendorNote">The vendor note</param>
        void DeleteVendorNote(VendorNote vendorNote);

        /// <summary>
        /// Formats the vendor note text
        /// </summary>
        /// <param name="vendorNote">Vendor note</param>
        /// <returns>Formatted text</returns>
        string FormatVendorNoteText(VendorNote vendorNote);

        /// <summary>
        /// Get All Top 'amount' Vendors (vendors having the highes average reviews)
        /// </summary>
        List<Vendor> GetAllTopXVendors(int amount);

        /// <summary>
        /// Get Most Popular 'amount' of Vendors (vendors having the highes number of followers)
        /// </summary>
        List<Vendor> GetMostPopularVendors(int amount);

        /// <summary>
        /// Get all products belonging to vendorId
        /// </summary>
        /// <param name="vendorId"></param>
        IEnumerable<Product> GetAllVendorProducts(int vendorId);

        IEnumerable<VendorReviewRecord> GetVendorReviews(int vendorId);

        IEnumerable<VendorReviewRecord> GetVendorQuestions(int vendorId, int amount);

        IEnumerable<VendorCustomer> GetVendorFavouriteCustomers(int vendorId, int amount);
        IEnumerable<VendorCustomerStory> GetVendorStories(int vendorId, int amount);

        IEnumerable<VendorCustomerStory> GetVendorChat(int vendorId, int customerId);

        void SaveVendorStories(int vendorId, int customerId, string questionText, bool isOwnStory);
        void SaveFollower(int vendorId, int customerId);
        int GetNumberOfFollowers(int vendorId);

        IEnumerable<VendorFaq> GetVendorFaqs(int vendorId, int amount);

        IEnumerable<Vendor> GetTopCategoryVendors(int categoryId, int amount);

        IEnumerable<Vendor> GetCategoryVendors(int categoryId);

        int GetNumberOfVendorsSellingCategory(int categoryId);

        IEnumerable<VendorPictureRecord> GetVendorFeaturette(string vendorName);

        string GetVendorMostSoldProduct(int vendorId);
    }
}