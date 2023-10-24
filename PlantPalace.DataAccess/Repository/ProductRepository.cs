using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private  ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var objFormDb =  _db.Products.FirstOrDefault(u => u.Id == obj.Id);

            if (objFormDb != null)
            {
                objFormDb.Name = obj.Name;
                objFormDb.Description = obj.Description;    
                objFormDb.Price = obj.Price;
                objFormDb.ListPrice = obj.ListPrice;
                objFormDb.Price50 = obj.Price50;
                objFormDb.Price100 = obj.Price100;
                objFormDb.categoryId = obj.categoryId;
                if(objFormDb.ImageUrl != null)
                {
                    objFormDb.ImageUrl = obj.ImageUrl;
                }
            }
        }

    }
}
