using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantPalace.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }

        IApplicationUserRepository ApplicationUser { get; }

        IShoppingCartRepository ShoppingCart { get; }
        IWishListRepository WishList { get; }

        IOrderDetailRepository OrderDetail { get; }

        IOrderHeaderRepository OrderHeader { get; }

        void Save();
    }
}
