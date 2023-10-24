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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private  ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int QuantityDecrement(ShoppingCart cart, int Quantity)
        {
            cart.Quantity -= Quantity;
            return cart.Quantity;
        }

        public int QuantityIncrement(ShoppingCart cart, int Quantity)
        {
            cart.Quantity += Quantity;
            return cart.Quantity;
        }
    }
}
