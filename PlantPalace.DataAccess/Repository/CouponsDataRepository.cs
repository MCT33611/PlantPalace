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
    public class CouponsDataRepository : Repository<CouponData>, ICouponsDataRepository
    {
        private  ApplicationDbContext _db;
        public CouponsDataRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        
    }
}
