using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
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
        public IProductRepository Product { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IWishListRepository WishList { get; set; }
        public IApplicationUserRepository ApplicationUser { get; set; }

		public IOrderDetailRepository OrderDetail { get; set; }

		public IOrderHeaderRepository OrderHeader { get; set; }

        public IBannerRepository Banner { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            WishList = new WishListRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            OrderDetail = new OrderDetailRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);

            Banner = new BannerRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
