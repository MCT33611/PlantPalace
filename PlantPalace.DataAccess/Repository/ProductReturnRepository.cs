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
    public class ProductReturnRepository : Repository<ProductReturn>, IProductReturnRepository
    {
        private ApplicationDbContext _db;
        public ProductReturnRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void UpdateStatus(int id, string returnStatus)
        {
            var ReturnDb = _db.ProductReturn.FirstOrDefault(u => u.Id == id);
            if (ReturnDb != null)
            {
                ReturnDb.ReturnStatus = returnStatus;
            }
        }


    }
}
