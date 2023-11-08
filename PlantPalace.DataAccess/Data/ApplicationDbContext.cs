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
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
		public DbSet<WishList> WishList { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }

        public DbSet<Banner> Banners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "Indoor", Tax = 18 },

                new Category { Id = 2, Name = "Outdoor", Tax = 18 },

                new Category { Id = 3, Name = "Tree", Tax = 18 },

                new Category { Id = 4, Name = "Hanging", Tax = 18 }




                );
            modelBuilder.Entity<Banner>().HasData(
                new Banner
                {
                    Id = 1,
                    BannerUrl = "/images/banners/Up to 70% off .jpg",
                    BannerName = "Banner 1",
                    Description = "Description for Banner 1"
                },
                new Banner
                {   
                    Id=2,
                    BannerUrl = "/images/banners/Zephyranthes Bulbs.jpg",
                    BannerName = "Banner 2",
                    Description = "Description for Banner 2"
                }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1,
                    Name = "Product 1",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 1,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10
                },
                new Product { Id = 2,
                    Name = "Product 2",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 4,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 3,
                    Name = "Product 3",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 2,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 4,

                    Name = "Product 4",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75, 
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 3,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 5,
                    Name = "Product 5",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 1,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 6,
                    Name = "Product 6",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 1,
                    ImageUrl = "",
                    Stock = 10
                },
                new Product { Id = 7,
                    Name = "Product 7",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 3,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 8,

                    Name = "Product 8",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 2,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 9,
                    Name = "Product 9",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 4,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                },
                new Product { Id = 10,
                    Name = "Product 10",
                    ListPrice = 100.00,
                    Price = 95.00,
                    Price50 = 90.75,
                    Price100 = 85.00,
                    Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    categoryId = 3,
                    ImageUrl = "https://www.plantsguru.com/image/cache/pg-flowering-plants-175x175.jpg",
                    ImageOne = "https://www.plantsguru.com/image/cache/Aquatic/plantsguru-adenium-light-of-sun-and-moon-400x400.jpg",
                    ImageTwo = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    ImageThree = "https://www.plantsguru.com/image/cache/catalog/Desertrose/pg-adenium-phet-mong-kon-400x400.jpg",
                    Stock = 10

                }
                );

        }
    }
}