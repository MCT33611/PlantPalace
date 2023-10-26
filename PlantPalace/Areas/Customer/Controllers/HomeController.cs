using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace PlantPalaceWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;


        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;

        }

        [HttpGet]
        public IActionResult OTP()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResentOtpAsync()
        {
            if(OTPM.Email != null)
            {
                await _emailSender.SendEmailAsync(OTPM.Email, "Confirm your email",
                        $"Your Otp is <a>{OTPM.OTP}</a>.");
            }
            else
            {
                ModelState.AddModelError("", "Sumting went worong ! Please refresh");
            }

            return RedirectToAction("OTP");
        }

        [HttpPost]
        public IActionResult OTP(string first, string second, string third, string fourth, string fifth, string sixth)
        {
            
            string userEnteredOTP = $"{first}{second}{third}{fourth}{fifth}{sixth}";

            
            string correctOTP = OTPM.OTP.ToString();

            if (userEnteredOTP == correctOTP)
            {
                
                
                return LocalRedirect("~/");
            }
            else
            {
                ViewBag.ErrorMessage = "Incorrect OTP. Please try again.";
                return View(); 
            }
        }


        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetALL(incluedProperties: "Category");
            return View(products);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, incluedProperties: "Category"),
                Quantity = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Details(ShoppingCart cart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.userId = claim.Value;

            ShoppingCart cartDb =
                _unitOfWork.ShoppingCart.Get(u => u.userId == claim.Value && u.ProductId == cart.ProductId);
                
            if(cartDb == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.QuantityIncrement(cartDb,cart.Quantity);

            }

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}