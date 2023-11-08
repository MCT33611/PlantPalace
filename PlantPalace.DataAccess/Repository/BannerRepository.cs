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
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private  ApplicationDbContext _db;
        public BannerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Banner obj)
        {
            _db.Banners.Update(obj);
        }

    }
}
