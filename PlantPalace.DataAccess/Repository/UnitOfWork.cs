using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public ICouponRepository Coupon { get;  set; }
        public ICouponsDataRepository CouponsData { get; set; }

        public IProductRepository Product { get; private set; }
        public IProductReturnRepository ProductReturn { get; set; }
        public IProductReviewRepository ProductReview { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IWishListRepository WishList { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }
        public IWalletTransactionRepository WalletTransaction { get;  set; }


        public IOrderDetailRepository OrderDetail { get; set; }

		public IOrderHeaderRepository OrderHeader { get; set; }

        public IOfferRepository Offer { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            ProductReview = new ProductReviewRepository(_db);
            ProductReturn = new ProductReturnRepository(_db);
            Category = new CategoryRepository(_db);
            Coupon = new CouponRepository(_db);
            CouponsData = new CouponsDataRepository(_db);
            WalletTransaction = new WalletTransactionRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            WishList = new WishListRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);

            Offer = new OfferRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
