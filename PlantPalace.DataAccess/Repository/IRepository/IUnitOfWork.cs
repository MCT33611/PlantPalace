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
        ICouponRepository Coupon { get; }
        ICouponsDataRepository CouponsData { get; }
        IProductRepository Product { get; }
        IProductReviewRepository ProductReview { get; }

        IApplicationUserRepository ApplicationUser { get; }
        IWalletTransactionRepository WalletTransaction { get; }


        IShoppingCartRepository ShoppingCart { get; }
        IWishListRepository WishList { get; }

        IOrderDetailRepository OrderDetail { get; }

        IOrderHeaderRepository OrderHeader { get; }
        IOfferRepository Offer { get; }

        void Save();
    }
}
