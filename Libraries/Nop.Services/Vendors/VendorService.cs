using Microsoft.EntityFrameworkCore;
using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Data.Extensions;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Core.Html;
using Nop.Data;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Nop.Services.Vendors
{
    /// <summary>
    /// Vendor service
    /// </summary>
    public partial class VendorService : IVendorService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<Vendor> _vendorRepository;
        private readonly IRepository<VendorNote> _vendorNoteRepository;
        private readonly IDbContext _dbContext;
        private readonly IDataProvider _dataProvider;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductVendor> _productVendorRepository;
        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<VendorReviewRecord> _vendorReviewRepository;
        private readonly IRepository<VendorPictureRecord> _vendorPictureRepository;
        private readonly IRepository<VendorCustomer> _vendorCustomerRepository;
        private readonly IRepository<VendorCustomerStory> _vendorCustomerStoryRepository;
        private readonly IRepository<Follower> _followerRepository;
        private readonly IRepository<VendorFaq> _vendorFaqRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Core.Domain.Orders.Order> _orderRepository;


        #endregion

        #region Ctor

        public VendorService(IEventPublisher eventPublisher,
            IRepository<Vendor> vendorRepository,
            IRepository<VendorNote> vendorNoteRepository,
            IDbContext dbContext,
            IDataProvider dataProvider,
            IRepository<Product> productRepository,
            IRepository<ProductVendor> productVendorRepository,
            IRepository<VendorReviewRecord> vendorReviewRepository,
            IRepository<VendorCustomer> vendorCustomerRepository,
            IRepository<VendorCustomerStory> vendorCustomerStoryRepository,
            IRepository<Follower> followerRepository,
            IRepository<VendorFaq> vendorFaqRepository,
            IRepository<ProductCategory> productCategoryRepository,
            IRepository<Category> categoryRepository,
            IRepository<VendorPictureRecord> vendorPictureRepository,
            IRepository<Core.Domain.Orders.Order> orderRepository)
        {
            this._eventPublisher = eventPublisher;
            this._vendorRepository = vendorRepository;
            this._vendorNoteRepository = vendorNoteRepository;
            this._dbContext = dbContext;
            this._dataProvider = dataProvider;
            this._productRepository = productRepository;
            this._productVendorRepository = productVendorRepository;
            this._vendorReviewRepository = vendorReviewRepository;
            this._vendorCustomerRepository = vendorCustomerRepository;
            this._vendorCustomerStoryRepository = vendorCustomerStoryRepository;
            this._followerRepository = followerRepository;
            this._vendorFaqRepository = vendorFaqRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._categoryRepository = categoryRepository;
            this._vendorPictureRepository = vendorPictureRepository;
            this._orderRepository = orderRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a vendor by vendor identifier
        /// </summary>
        /// <param name="vendorId">Vendor identifier</param>
        /// <returns>Vendor</returns>
        public virtual Vendor GetVendorById(int vendorId)
        {
            if (vendorId == 0)
                return null;

            return _vendorRepository.GetById(vendorId);
        }

        public virtual Vendor GetVendorByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = _vendorRepository.Table.Include(vn => vn.VendorNotes)
                .Where(v => v.Email == email);

            var vendor = query.FirstOrDefault();
            return vendor;
        }

        /// <summary>
        /// Delete a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void DeleteVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            vendor.Deleted = true;
            UpdateVendor(vendor);

            //event notification
            _eventPublisher.EntityDeleted(vendor);
        }

        /// <summary>
        /// Gets all vendors
        /// </summary>
        /// <param name="name">Vendor name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Vendors</returns>
        public virtual IPagedList<Vendor> GetAllVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _vendorRepository.Table;
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);

            query = query.Where(v => !v.Deleted);
            query = query.OrderBy(v => v.DisplayOrder).ThenBy(v => v.Name);

            var vendors = new PagedList<Vendor>(query, pageIndex, pageSize);
            return vendors;
        }

        public virtual IPagedList<Vendor> GetAllMostPopularVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var amountParameter = _dataProvider.GetInt32Parameter("Amount", int.MaxValue);
            var query = _dbContext.EntityFromSql<Vendor>("GetMostPopularVendors", amountParameter);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);

            query = query.Where(v => !v.Deleted);

            var vendors = new PagedList<Vendor>(query, pageIndex, pageSize);
            return vendors;
        }

        public virtual IPagedList<Vendor> GetAllBestVendors(string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var amountParameter = _dataProvider.GetInt32Parameter("VendorAmount", int.MaxValue);
            var query = _dbContext.EntityFromSql<Vendor>("GetTopXVendors", amountParameter);
            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(v => v.Name.Contains(name));
            if (!showHidden)
                query = query.Where(v => v.Active);

            query = query.Where(v => !v.Deleted);

            var vendors = new PagedList<Vendor>(query, pageIndex, pageSize);
            return vendors;
        }

        public virtual IPagedList<Vendor> GetAllVendorsForCategory(int categoryId, string name = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var categoryVendors = this.GetCategoryVendors(categoryId);
            var vendors = new PagedList<Vendor>(categoryVendors.AsQueryable(), pageIndex, pageSize);
            return vendors;
        }

        /// <summary>
        /// Get All Top 'amount' Vendors (vendors having the highes average reviews)
        /// </summary>
        /// <param name="amount">the amount of vendors to return</param>
        /// <returns></returns>
        public virtual List<Vendor> GetAllTopXVendors(int amount)
        {
            var vendors = _dbContext.EntityFromSql<Vendor>("GetTopXVendors", amount).ToList();
            return vendors;
        }

        /// <summary>
        /// Get Most Popular 'amount' Vendors (vendors having the highes number of followers)
        /// </summary>
        /// <param name="amount">the amount of vendors to return</param>
        /// <returns></returns>
        public virtual List<Vendor> GetMostPopularVendors(int amount)
        {
            var amountParameter = _dataProvider.GetInt32Parameter("Amount", amount);
            var vendors = _dbContext.EntityFromSql<Vendor>("GetMostPopularVendors", amountParameter).ToList();
            return vendors;
        }

        /// <summary>
        /// Gets vendors
        /// </summary>
        /// <param name="vendorIds">Vendor identifiers</param>
        /// <returns>Vendors</returns>
        public virtual IList<Vendor> GetVendorsByIds(int[] vendorIds)
        {
            var query = _vendorRepository.Table;
            if (vendorIds != null)
                query = query.Where(v => vendorIds.Contains(v.Id));

            return query.ToList();
        }

        /// <summary>
        /// Inserts a vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void InsertVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            _vendorRepository.Insert(vendor);

            //event notification
            _eventPublisher.EntityInserted(vendor);
        }

        /// <summary>
        /// Updates the vendor
        /// </summary>
        /// <param name="vendor">Vendor</param>
        public virtual void UpdateVendor(Vendor vendor)
        {
            if (vendor == null)
                throw new ArgumentNullException(nameof(vendor));

            _vendorRepository.Update(vendor);

            //event notification
            _eventPublisher.EntityUpdated(vendor);
        }

        /// <summary>
        /// Gets a vendor note
        /// </summary>
        /// <param name="vendorNoteId">The vendor note identifier</param>
        /// <returns>Vendor note</returns>
        public virtual VendorNote GetVendorNoteById(int vendorNoteId)
        {
            if (vendorNoteId == 0)
                return null;

            return _vendorNoteRepository.GetById(vendorNoteId);
        }

        /// <summary>
        /// Deletes a vendor note
        /// </summary>
        /// <param name="vendorNote">The vendor note</param>
        public virtual void DeleteVendorNote(VendorNote vendorNote)
        {
            if (vendorNote == null)
                throw new ArgumentNullException(nameof(vendorNote));

            _vendorNoteRepository.Delete(vendorNote);

            //event notification
            _eventPublisher.EntityDeleted(vendorNote);
        }

        public virtual void UpdateVendorNote(VendorNote vendorNote)
        {
            if (vendorNote == null)
                throw new ArgumentNullException(nameof(vendorNote));           
            var vendorNotes = _vendorNoteRepository.Table
                .Where(v => v.VendorId == vendorNote.VendorId)
                .ToList();
            _vendorNoteRepository.Delete(vendorNotes);
            _vendorNoteRepository.Insert(vendorNote);
        }

        /// <summary>
        /// Formats the vendor note text
        /// </summary>
        /// <param name="vendorNote">Vendor note</param>
        /// <returns>Formatted text</returns>
        public virtual string FormatVendorNoteText(VendorNote vendorNote)
        {
            if (vendorNote == null)
                throw new ArgumentNullException(nameof(vendorNote));

            var text = vendorNote.Note;

            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);

            return text;
        }


        public IEnumerable<Product> GetAllVendorProducts(int vendorId)
        {
            //get all product id belonging to that vendor
            var vendorProducts = _productRepository.Table.Where(v => v.VendorId == vendorId).ToList();
            return vendorProducts;
        }

        public IEnumerable<VendorReviewRecord> GetVendorReviews(int vendorId)
        {
            //get all reviews belonging to that vendor
            var vendorReviews = _vendorReviewRepository.Table
                                    .Where(v => v.VendorId == vendorId &&
                                           v.IsApproved == true &&
                                           v.IsQuestion == null || v.IsQuestion == false)
                                    .ToList();
            return vendorReviews;
        }

        public IEnumerable<Vendor> GetVendorsFromReviewsAndCustomer(int customerId)
        {
            var vendorReviews = _vendorReviewRepository.Table
                                    .Where(v => v.CustomerId == customerId &&
                                           v.IsApproved == true &&
                                           v.IsQuestion == null || v.IsQuestion == false)
                                    .Select(v => v.Vendor)
                                    .Distinct()
                                    .ToList();
            return vendorReviews;
        }

        public IEnumerable<Vendor> GetVendorsFromCustomerPurchasedItems(int customerId)
        {
            var orderItems = _orderRepository.Table.Include(o => o.OrderItems)
                .ThenInclude(v => v.Product)
                .ThenInclude(v => v.ProductVendors)
                .ThenInclude(v => v.Vendor)
                .Where(v => v.CustomerId == customerId)
                .Select(v => v.OrderItems)
                .Distinct()
                .ToList();
            var productVendors = new List<int>();
            foreach (var item in orderItems)
            {
                foreach (var i in item)
                {
                    productVendors.Add(i.Product.VendorId);
                }
            }

            var vendors = from v in _vendorRepository.Table
                          where productVendors.Contains(v.Id)
                          select v;

            return vendors.Distinct().ToList();
        }

        /// <summary>
        /// TODO: needs testing
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="vendorId"></param>
        /// <returns></returns>
        public int GetNumberOfPurchasedItems(int customerId, int vendorId)
        {
            var orderItems = _orderRepository.Table.Include(o => o.OrderItems)
                .ThenInclude(v => v.Product)
                .ThenInclude(v => v.ProductVendors)
                .ThenInclude(v => v.Vendor)
                .Where(v => v.CustomerId == customerId)
                .Select(v => v.OrderItems)
                .Distinct()
                .ToList();
            int count = 0;
            foreach (var item in orderItems)
            {
                var itemsFromVendor = item.Where(i => i.Product.VendorId == vendorId).ToList();
                count += itemsFromVendor.Count();
            }

            return count;
        }

        public IEnumerable<VendorReviewRecord> GetVendorQuestions(int vendorId, int amount)
        {
            //get all reviews belonging to that vendor
            var vendorReviews = _vendorReviewRepository.Table
                                    .Where(v => v.VendorId == vendorId &&
                                                v.IsApproved == true &&
                                                v.IsQuestion == true)
                                    .Take(amount)
                                    .ToList();
            return vendorReviews;
        }

        public IEnumerable<VendorCustomer> GetVendorFavouriteCustomers(int vendorId, int amount)
        {
            //get all fav. customers belonging to that vendor
            var vendorCustomers = _vendorCustomerRepository.Table
                                    .Where(v => v.VendorId == vendorId &&
                                                v.IsFavourite == true)
                                    .Take(amount)
                                    .ToList();
            return vendorCustomers;
        }

        public IEnumerable<VendorCustomerStory> GetVendorStories(int vendorId, int amount)
        {
            var vendorCustomerStories = _vendorCustomerStoryRepository.Table
                                    .Where(v => v.VendorId == vendorId &&
                                                v.IsApproved == true)
                                    .Take(amount)
                                    .ToList();

            return vendorCustomerStories;
        }

        public IEnumerable<VendorCustomerStory> GetVendorChat(int vendorId, int customerId)
        {
            var vendorCustomerStories = _vendorCustomerStoryRepository.Table
                                    .Where(v => v.VendorId == vendorId &&
                                                v.CustomerId == customerId)
                                    .ToList();

            return vendorCustomerStories;
        }

        public void SaveVendorStories(int vendorId, int customerId, string questionText, bool isOwnStory)
        {
            var vendorCustomerStories = new VendorCustomerStory
            {
                VendorId = vendorId,
                CustomerId = customerId,
                QuestionText = questionText,
                IsOwnStory = isOwnStory,
                StoreId = 1,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedOnUtc = DateTime.UtcNow
            };

            _vendorCustomerStoryRepository.Insert(vendorCustomerStories);
        }

        public void SaveFollower(int vendorId, int customerId)
        {
            //follower already assigned
            if (_followerRepository.Table.Any(f => f.VendorId == vendorId && f.CustomerId == customerId))
                return;

            var follower = new Follower
            {
                VendorId = vendorId,
                CustomerId = customerId
            };

            _followerRepository.Insert(follower);

            var vendor = _vendorRepository.Table.Where(v => v.Id == vendorId).FirstOrDefault();
            if (vendor != null)
            {
                if (vendor.FollowersNumber == null)
                {
                    vendor.FollowersNumber = 1;
                }
                else
                {
                    vendor.FollowersNumber++;
                }

                _vendorRepository.Update(vendor);
            }
        }

        public int GetNumberOfFollowers(int vendorId)
        {
            return _followerRepository.Table.Where(vendor => vendor.Id == vendorId).Count();
        }

        public IEnumerable<VendorFaq> GetVendorFaqs(int vendorId, int amount)
        {
            //get all faqs belonging to that vendor shop
            var vendorFaqs = _vendorFaqRepository.Table
                                    .Where(v => v.VendorId == vendorId)
                                    .Take(amount)
                                    .ToList();
            return vendorFaqs;
        }

        public IEnumerable<Vendor> GetTopCategoryVendors(int categoryId, int amount)
        {
            //get all sub categories belonging to that category
            var categoryIds = _categoryRepository.Table
                                    .Where(cat => cat.ParentCategoryId == categoryId)
                                    .Select(cat => cat.Id)
                                    .ToList();

            categoryIds.Add(categoryId);

            var query = _productCategoryRepository.Table
                        .Where(pc => categoryIds.Contains(pc.CategoryId))
                        .GroupBy(pc => pc.Product.VendorId)
                        .Select(group =>
                                new
                                {
                                    VendorId = group.Key,
                                    AmountSold = group.Sum(x => x.Product.AmountSold)
                                })
                        .ToList();

            var vendorIds = query
                            .OrderByDescending(q => q.AmountSold)
                            .Take(amount)
                            .OrderBy(q => Guid.NewGuid())
                            .Select(q => q.VendorId);

            return _vendorRepository.Table
                    .Where(v => vendorIds.Contains(v.Id))
                    .OrderByDescending(v => v.IsPremium)
                    .ToList();
        }

        public IEnumerable<Vendor> GetCategoryVendors(int categoryId)
        {
            var vendorIds = _productCategoryRepository.Table
                        .Where(pc => pc.CategoryId == categoryId)
                        .Select(pc => pc.Product.VendorId)
                        .Distinct()
                        .ToList();

            return _vendorRepository.Table
                    .Where(v => vendorIds.Contains(v.Id))
                    .ToList();
        }

        public int GetNumberOfVendorsSellingCategory(int categoryId)
        {
            var numberOfVendors = _productCategoryRepository.Table
                        .Where(pc => pc.CategoryId == categoryId)
                        .Select(pc => pc.Product.VendorId)
                        .Where(v => v > 0)
                        .Distinct()
                        .Count();

            return numberOfVendors;
        }

        public IEnumerable<VendorPictureRecord> GetVendorFeaturette(string vendorName)
        {
            return _vendorPictureRepository.Table
                                           .Where(vp => vp.Vendor.Name == vendorName)
                                           .Take(2)
                                           .ToList();
        }

        public string GetVendorMostSoldProduct(int vendorId)
        {
            var mostSoldProduct = _productRepository.Table
                                                    .Where(p => p.VendorId == vendorId)
                                                    .OrderByDescending(p => p.AmountSold)
                                                    .FirstOrDefault();

            return mostSoldProduct != null ? mostSoldProduct.Name : string.Empty;
        }

        #endregion
    }
}