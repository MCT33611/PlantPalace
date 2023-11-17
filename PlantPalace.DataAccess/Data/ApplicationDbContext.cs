using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlantPalace.Models;

namespace PlantPalace.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
        {
        }

        public DbSet<Category> Categorries { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<CouponData> CouponsData { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReview> ProductReview { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<WalletTransaction> WalletTransaction { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<WishList> WishList { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<Banner> Banners { get; set; }
        /*public object Coupon { get; internal set; }*/
    }
}