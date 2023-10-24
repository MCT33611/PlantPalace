using PlantPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int QuantityIncrement(ShoppingCart cart,int Quantity);
        int QuantityDecrement(ShoppingCart cart,int Quantity);
    }
}
 