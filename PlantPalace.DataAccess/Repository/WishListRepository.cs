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
    public class WishListRepository : Repository<WishList>, IWishListRepository
    {
        private  ApplicationDbContext _db;
        public WishListRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


    }
}
