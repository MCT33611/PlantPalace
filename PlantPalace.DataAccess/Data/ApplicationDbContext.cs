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
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderHeader> OrderHeader { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(

                new Category { Id = 1, Name = "Indoor", DisplayOrder = 1 },

                new Category { Id = 2, Name = "Outdoor", DisplayOrder = 1 },

                new Category { Id = 3, Name = "Tree", DisplayOrder = 1 },

                new Category { Id = 4, Name = "Hanging", DisplayOrder = 1 }




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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
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
                    ImageUrl = "",
                    Stock = 10

                }
                );

        }
    }
}