using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PlantPalace.DataAccess.Repository;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
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

        [HttpGet]
        public async Task<IActionResult> ResentOtp()
        {
            if(OTPM.Email != null)
            {
                OTPM.GenerateOTP();
                await _emailSender.SendEmailAsync(OTPM.Email, "Confirm your email",
                        $"Your Otp is <a>{OTPM.OTP}</a>.");
            }
            else
            {
                ModelState.AddModelError("", "Sumting went worong ! Please refresh");
            }

            return RedirectToAction(nameof(OTP));
            
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
            if (User.IsInRole(SD.Role_Admin))
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            HomeVM homeModel = new()
            {
                products = _unitOfWork.Product.GetALL(incluedProperties: "Category"),

                banners = _unitOfWork.Offer.GetALL(u=> u.OfferType == "banner"),

                categories = _unitOfWork.Category.GetALL(),

                orderDetails = _unitOfWork.OrderDetail.GetALL(incluedProperties: "OrderHeader,Product"),

            };

            
            return View(homeModel);
        }

        /*public IActionResult ProductList(int[]? Rate, int[]? Categories, int? priceRange, string? search)*/
        public IActionResult ProductList(ProductFilterVM model,string?offer)
        {
            ProductListVM list = new()
            {
                products = _unitOfWork.Product.GetALL().ToList(),
                Categories = _unitOfWork.Category.GetALL().ToList(),
            };
            var random = new Random();
            list.products = list.products.OrderBy(x => random.Next()).ToList();
            int[] rates = JsonConvert.DeserializeObject<int[]>(model.Rates ?? "[]");
            int[] categories = JsonConvert.DeserializeObject<int[]>(model.Categories ?? "[]");

            if (!string.IsNullOrEmpty(offer))
            {
                list.products=list.products.Where(u => u.OfferName.Normalize() == offer.Normalize()).ToList();
            }


            if ((model.Rates?.Any() == true) || (model.Categories?.Any() == true) || (model.PriceRange.HasValue && model.PriceRange != 0) || !string.IsNullOrEmpty(model.search))
            {
                try
                {
                    if (rates?.Any() == true)
                    {
                        list.products = list.products.Where(u => u.Rate >= rates.Min()).ToList();
                    }

                    if (categories?.Any() == true)
                    {
                        
                        list.products = list.products.Where(u => categories.Contains(u.categoryId)).ToList();
                    }


                    if (model.PriceRange.HasValue && model.PriceRange != 0)
                    {
                        list.products = list.products.Where(u => u.Price <= model.PriceRange).ToList();
                    }

                    if (!string.IsNullOrEmpty(model.search))
                    {
                        list.products = list.products.Where(u =>
                            u.Name.Normalize().Contains(model.search.Normalize()) ||
                            u.Category.Name.Normalize().Contains(model.search.Normalize()) ||
                            u.SubCategory.Normalize().Contains(model.search.Normalize())
                        ).ToList();
                    }

                    if (!string.IsNullOrEmpty(model.search))
                    {
                        list.products = list.products.Where(u =>
                            u.Name.Normalize().Contains(model.search.Normalize()) ||
                            u.Category.Name.Normalize().Contains(model.search.Normalize()) ||
                            u.SubCategory.Normalize().Contains(model.search.Normalize())
                        ).ToList();
                    }

                    

                    return PartialView("_ProductListPartial", list.products.ToList() );
                }
                catch (Exception ex)
                {
                    // Log the exception details
                    _logger.LogError(ex, "An error occurred in the ProductList action.");

                    // Return a generic error response
                    return StatusCode(500, "Internal Server Error");
                }

            }

            return View(list);
        }


        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, incluedProperties: "Category"),
                Quantity = 1,
                ProductId = productId
            };
            DetailsVM detailsVM = new()
            {
                cart = cart,
                reviewList = _unitOfWork.ProductReview.GetALL(incluedProperties: "User").ToList(),
                eligible = false
            };
            if(User.IsInRole(SD.Role_Customer))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                detailsVM.eligible = _unitOfWork.ProductReview.GetALL(u => u.ProductId == productId && u.UserId == claim.Value).Count() == 0;

            }
            return View(detailsVM);
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
                if (!_unitOfWork.Product.IsStockAvailable(1, cart.ProductId))
                {
                    TempData["error"] = "Stock is not more";

                    return RedirectToAction(nameof(Index));

                }
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                if (!_unitOfWork.Product.IsStockAvailable(cartDb.Quantity, cart.ProductId))
                {
                    TempData["error"] = "Stock is not more";

                    return RedirectToAction(nameof(Index));

                }
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




        #region API CALLS

        public IActionResult PicUpload(IFormFile file, string FORfilename)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);
            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                
                return RedirectToAction(nameof(Index));
            }

            if (file != null && file.Length > 0)
            {
                // Define a folder path to save user photos
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/profiles");

                // Ensure the folder exists
                Directory.CreateDirectory(uploadFolder);

                // Generate a unique file name for the photo
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                fileName += fileName + '_' + FORfilename;
                // Combine the folder path and file name
                var filePath = Path.Combine(uploadFolder, fileName);


                // if user profile alldriady exist it delete


                if (!string.IsNullOrEmpty(user.Pic))
                {
                    var oldImagePath = Directory.GetCurrentDirectory() + "\\wwwroot\\" + user.Pic.TrimStart('\\');

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                ImageCrop crop = new();

                crop.Crop(filePath, file);
                // Save the photo to the server
                /*using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }*/

                // Store the file path in the database (update your database model accordingly)
                user.Pic = "/Images/profiles\\" + fileName; // Update the user's profile photo property in the database

                
            }

            _unitOfWork.ApplicationUser.Update(user);
            _unitOfWork.Save();
            return Json(new {success = true});
        }
        [HttpGet]
        [Authorize]
        public IActionResult AddTOcart(int ProductId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Quantity = 1,
                ProductId = ProductId
            };


            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            cart.userId = claim.Value;

            

            ShoppingCart cartDb =
                _unitOfWork.ShoppingCart.Get(u => u.userId == claim.Value && u.ProductId == cart.ProductId);

            

            if (cartDb == null)
            {
                if (!_unitOfWork.Product.IsStockAvailable( 1, cart.ProductId))
                {
                    TempData["error"] = "Stock is not more";

                    return RedirectToAction(nameof(Index));

                }
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                if (!_unitOfWork.Product.IsStockAvailable(cartDb.Quantity + 1, cart.ProductId))
                {
                    TempData["error"] = "Stock is not more";

                    return RedirectToAction(nameof(Index));

                }
                _unitOfWork.ShoppingCart.QuantityIncrement(cartDb, 1);

            }

            _unitOfWork.Save();
            return Json(new { success = true, message = "Product Added to Cart" });



        }


        [HttpPost]
        [Authorize]
        public IActionResult AddReview(int rate,string review, int productId)
        {
            try
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var user = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

                if (user == null)
                    return NotFound();
                ProductReview reviewModel = new()
                {
                    Rate = rate,
                    Description = review,
                    ProductId = productId,
                    UserId = user.Id,
                    CreatedDate = DateTime.UtcNow,

                };

                var product = _unitOfWork.Product.Get(u=>u.Id == productId);
                product.Rate = _unitOfWork.ProductReview.GetALL(u=> u.ProductId == productId).Sum(u => u.Rate) / _unitOfWork.ProductReview.GetALL(u => u.ProductId == productId).Count();
                if (reviewModel != null)
                {
                    _unitOfWork.ProductReview.Add(reviewModel);
                }
                else
                {
                    throw new Exception();
                }

                _unitOfWork.Save();
                TempData["success"] = "review Send Successfully";

                return RedirectToAction(nameof(Details), new {productId});
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Details), new { productId });

            }
        }

        #endregion
    }

}