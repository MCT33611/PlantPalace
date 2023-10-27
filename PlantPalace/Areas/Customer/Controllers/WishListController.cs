using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;
using Stripe.Checkout;
using System.Security.Claims;

namespace PlantPalaceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class WishListController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public WishListVM WishListVM { get; set; }
        public WishListController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            WishListVM = new WishListVM()
            {

                WishLists = _unitOfWork.WishList.GetALL(u => u.userId == claim.Value, incluedProperties: "Product"),
            };

            return View(WishListVM);
        }


        [HttpGet]
        [Authorize]
        public IActionResult AddORremove(int ProductId)
        {
            WishList listitem = new WishList()
            {
                ProductId = ProductId
            };


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            listitem.userId = claim.Value;



            WishList listDb =
                _unitOfWork.WishList.Get(u => u.userId == claim.Value && u.ProductId == listitem.ProductId);

            if (listDb == null)
            {
                _unitOfWork.WishList.Add(listitem);
            }
            else
            {
                _unitOfWork.WishList.Remove(listDb);


            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));


        }
    }
}
