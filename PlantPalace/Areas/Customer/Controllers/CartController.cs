using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;
using Stripe.Checkout;
using System.Security.Claims;
using PlantPalace.Utility;
using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace PlantPalaceWeb.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }


		private readonly IEmailSender _emailSender;
		private readonly IRazorViewRenderer _viewRenderService;
		public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender, IRazorViewRenderer viewRenderService)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
			_viewRenderService = viewRenderService;
		}



		public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{

				ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product"),
				OrderHeader = new()
			};

			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100,cart.Product.DiscountPrice);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
			}
			return View(ShoppingCartVM);
		}


		public IActionResult Summary()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			ShoppingCartVM = new ShoppingCartVM()
			{

				ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product"),

				OrderHeader = new()

			};


			if (ShoppingCartVM.ListCart.Count() <= 0)
			{
				return RedirectToAction(nameof(Index));
			}
			ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

			ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
			ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
			ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAdderss;
			ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
			ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
			ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

			foreach (var cart in ShoppingCartVM.ListCart)
			{
				cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100, cart.Product.DiscountPrice);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
			}

			HttpContext.Session.SetObject("ShoppingCartVM", ShoppingCartVM);

			return View(ShoppingCartVM);
		}


		[ActionName("Summary")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult SummaryPost(string PaymentMethod)
		{
            
            if (PaymentMethod == "OnlinePayment")
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				//ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product");

				var ShoppingCartVModel = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCartVM");

				ShoppingCartVM.ListCart = ShoppingCartVModel.ListCart;
				foreach (var cart in ShoppingCartVM.ListCart)
				{
					if (!_unitOfWork.Product.IsStockAvailable(cart.Quantity, cart.ProductId))
					{
						TempData["error"] = "Stock is not more";

						return View(ShoppingCartVM);

					}
				}
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.PaymentMethod = SD.PaymentMethodOnline;
				ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100, cart.Product.DiscountPrice);
					ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
				}

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					OrderDetail orderDetail = new()
					{
						ProductId = cart.ProductId,
						OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
						Price = cart.Price,
						Count = cart.Quantity,
					};
					_unitOfWork.OrderDetail.Add(orderDetail);
					_unitOfWork.Save();

				}

				var domain = "https://plantpalace.azurewebsites.net/";
				//var domain = "https://localhost:7253/";
                var options = new SessionCreateOptions
				{
					PaymentMethodTypes = new List<string>
				{
					"card",
				},
					LineItems = new List<SessionLineItemOptions>(),


					Mode = "payment",
					SuccessUrl = domain + $"customer/cart/OrderConfirmationOnline?id={ShoppingCartVM.OrderHeader.Id}",
					CancelUrl = domain + $"customer/cart/index",
				};

				foreach (var item in ShoppingCartVM.ListCart)
				{
					var sessionLineItem = new SessionLineItemOptions
					{
						PriceData = new SessionLineItemPriceDataOptions
						{
							UnitAmount = (long)(item.Price * 100),
							Currency = "inr",
							ProductData = new SessionLineItemPriceDataProductDataOptions
							{
								Name = item.Product.Name,
							},
						},
						Quantity = item.Quantity,
					};
					options.LineItems.Add(sessionLineItem);
				}

				var service = new SessionService();
				Session session = service.Create(options);

				_unitOfWork.OrderHeader.UpdateStripePaymentID(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();

				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}
			else if (PaymentMethod == "WalletPayment")
			{
				
                var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				//ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product");
				var ShoppingCartVModel = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCartVM");

				ShoppingCartVM.ListCart = ShoppingCartVModel.ListCart;


				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.PaymentMethod = SD.PaymentMethodWallet;
				ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100, cart.Product.DiscountPrice);
					ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
				}
                if (_unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value).WalletBalance < ShoppingCartVM.OrderHeader.OrderTotal)
                {
                    TempData["error"] = "Wallet Balance is not Enough for Payment Choose Other Method";
                    ModelState.AddModelError("summarySubmit", "Choose Other Method");
                    //return View();
                    return View(ShoppingCartVM);

                }

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					OrderDetail orderDetail = new()
					{
						ProductId = cart.ProductId,
						OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
						Price = cart.Price,
						Count = cart.Quantity,
					};
					_unitOfWork.OrderDetail.Add(orderDetail);
					_unitOfWork.Save();

				}

				
                _unitOfWork.ApplicationUser.UpdateWallet(claim.Value, -ShoppingCartVM.OrderHeader.OrderTotal);
				_unitOfWork.OrderHeader.Update(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				return RedirectToAction("OrderConfirmationOffline", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
			}
			else
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				//ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product");
				var ShoppingCartVModel = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCartVM");

				ShoppingCartVM.ListCart = ShoppingCartVModel.ListCart;

				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
				ShoppingCartVM.OrderHeader.PaymentMethod = SD.PaymentMethodCOD;
				ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
				ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

                /*foreach (var cart in ShoppingCartVM.ListCart)
                {
                    cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100,cart.Product.DiscountPrice);
                    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
                }*/

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					OrderDetail orderDetail = new()
					{
						ProductId = cart.ProductId,
						OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
						Price = cart.Price,
						Count = cart.Quantity,
					};
					_unitOfWork.OrderDetail.Add(orderDetail);
					_unitOfWork.Save();

				}



				_unitOfWork.OrderHeader.Update(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				return RedirectToAction("OrderConfirmationOffline", "Cart", new { id = ShoppingCartVM.OrderHeader.Id });
			}


		}

		public IActionResult OrderConfirmation(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, incluedProperties: "ApplicationUser");

			string mailBody = $@"


                                  <link rel=""stylesheet"" href=""~/lib/bootstrap/dist/css/bootstrap.css"" />

                                  <body>
                                    <div class=""container"">
                                        <div class=""row"">
                                            <div class=""col-md-6 mx-auto"">
                                                <div class=""card mt-5"">
                                                    <div class=""card-body"">
                                                        <h1 class=""card-title"">Order Confirmation</h1>
                                                        <p>Thank you for your order with PlantPalace!</p>
                                                        <p>Your order #{orderHeader.Id} has been confirmed.</p>
                                                        <p>Order Date: {orderHeader.OrderDate.ToShortDateString()}</p>                                                        <p>Order Date: {orderHeader.OrderDate.ToShortDateString()}</p>                                                        <p>Order Date: {orderHeader.OrderDate.ToShortDateString()}</p>
                                                        <p>Payment Method: {orderHeader.PaymentMethod}</p>

                                                        <p>Shipping Address:{orderHeader.State},{orderHeader.City},{orderHeader.StreetAddress}<BR/>PINCODE:{orderHeader.PostalCode}</p>
                                                        <p>Total Amount: {orderHeader.OrderTotal}</p>
                                                        
                
                        
                                                        <p>If you have any questions about your order, please contact our customer support at PlantPalace04@gmail.com or 8089342685.</p>
                        
                                                        <p>Thank you for shopping with us!</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </body>";

			_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "Order Place from PlantPalace", mailBody);

			List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetALL(u => u.userId == orderHeader.ApplicationUserId, incluedProperties: "Product").ToList();
			// Create a dictionary to store the product ID and its quantity in the shopping carts
			Dictionary<int, int> productQuantities = new Dictionary<int, int>();

			foreach (var cart in shoppingCarts)
			{
				// Assuming that the ShoppingCart model has a ProductId and Quantity property
				int productId = cart.ProductId;
				int quantity = cart.Quantity;

				// Update the productQuantities dictionary
				if (productQuantities.ContainsKey(productId))
				{
					productQuantities[productId] += quantity;
				}
				else
				{
					productQuantities[productId] = quantity;
				}
			}

			// Update product stock based on the quantities in the shopping carts
			foreach (var productId in productQuantities.Keys)
			{
				var product = _unitOfWork.Product.Get(u => u.Id == productId);

				if (product != null)
				{
					int quantityInCart = productQuantities[productId];

					// Check if the product stock is sufficient
					if (product.Stock >= quantityInCart)
					{
						// Update the product stock
						product.Stock -= quantityInCart;
						_unitOfWork.Product.Update(product);
					}
					else
					{
						// Handle insufficient stock (e.g., display an error message or take appropriate action)
						// You can return an error message or throw an exception here
					}
				}
			}
			_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
			_unitOfWork.Save();
			return View(id);
		}



		public IActionResult OrderConfirmationOffline(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, incluedProperties: "ApplicationUser");
			_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusPending);
			_unitOfWork.Save();


			return RedirectToAction("OrderConfirmation", "Cart", new { id = id });

		}

		public IActionResult OrderConfirmationOnline(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);

			if (session.PaymentStatus.ToLower() == "paid")
			{
				_unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusDelayedPayment);
				_unitOfWork.Save();
			}
			/*            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetALL(u => u.userId == orderHeader.ApplicationUserId).ToList();
                        _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
                        _unitOfWork.Save();*/

			return RedirectToAction("OrderConfirmation", "Cart", new { id = id });

		}

		public IActionResult Plus(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			if (!_unitOfWork.Product.IsStockAvailable(cart.Quantity + 1, cart.ProductId))
			{
				TempData["error"] = "no more Stock Avilable";

				return RedirectToAction("Index");

			}
			_unitOfWork.ShoppingCart.QuantityIncrement(cart, 1);
			_unitOfWork.Save();
			return RedirectToAction("Index");
		}

		public IActionResult Minus(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

			if (cart.Quantity <= 1)
			{
				_unitOfWork.ShoppingCart.Remove(cart);

			}
			else
			{
				_unitOfWork.ShoppingCart.QuantityDecrement(cart, 1);

			}
			_unitOfWork.Save();

			return RedirectToAction("Index");
		}
		public IActionResult Remove(int cartId)
		{
			var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
			_unitOfWork.ShoppingCart.Remove(cart);
			_unitOfWork.Save();

			return RedirectToAction("Index");
		}

		public IActionResult CouponApply(string couponCode)
		{
			try
			{
				var ShoppingCartVModel = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCartVM");
				var user = _unitOfWork.ApplicationUser.Get(c => c.Id == ShoppingCartVModel.OrderHeader.ApplicationUser.Id);

				var coupon = _unitOfWork.Coupon.Get(c => c.Code == couponCode);
				if (coupon == null)
				{
					TempData["error"] = "Coupon Is Not Exist ";
					ModelState.AddModelError("couponCode", "Coupon Is Not Exist");

					return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "ERROR" });


				}
				if (!_unitOfWork.Coupon.IsNotExpired(coupon.Id))
				{
					TempData["error"] = "Coupon Is Expired ";
					ModelState.AddModelError("couponCode", "Coupon Is Expired");

					return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "ERROR" });


				}
				if (ShoppingCartVModel.OrderHeader.OrderTotal < coupon.MinPrice)
				{
					TempData["error"] = $"This Coupen only applicable for atleast {coupon.MinPrice} total price";
					ModelState.AddModelError("couponCode", $"This Coupen only applicable for atleast {coupon.MinPrice} total price");

					return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "ERROR" });


				}
				if (user == null)
				{
					return NotFound();
				}
				if (coupon == null)
				{
					ModelState.AddModelError("couponCode", "Coupon Not Found Try Another");

					return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "ERROR" });


				}
				else
				{
					var couponData = _unitOfWork.CouponsData.Get(c => c.CouponId == coupon.Id && c.UserId == user.Id);
					if (couponData == null)
					{
						if (coupon.FixPrice != 0 && coupon.FixPrice != null)
						{
							ShoppingCartVModel.OrderHeader.OrderTotal -= (int)coupon.FixPrice;

						}
						else if (coupon.Percent != 0 && coupon.Percent != null)
						{
							ShoppingCartVModel.OrderHeader.OrderTotal -= (int)(ShoppingCartVModel.OrderHeader.OrderTotal * coupon.Percent / 100.0);

						}
						HttpContext.Session.SetObject("ShoppingCartVM", ShoppingCartVModel);

						CouponData data = new()
						{
							UserId = user.Id,
							CouponId = coupon.Id,
							Date = DateTime.UtcNow,
						};
						_unitOfWork.CouponsData.Add(data);
						_unitOfWork.Save();
						TempData["success"] = "Coupon Applied";

						return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "OK" });


					}
					else
					{
						TempData["error"] = "Coupon Already Used";
						ModelState.AddModelError("couponCode", "Coupon Already Used");
						return Json(new { OrderTotal = ShoppingCartVModel.OrderHeader.OrderTotal, message = "ERROR" });

					}
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				ModelState.AddModelError("couponCode", "SomeThing Went Wrong");
				return RedirectToAction(nameof(Summary), "Cart");

			}
		}
		private double GetPriceBasedOnQuantity(double quantity, double price, double price50, double price100,double discountPrice)
		{
			if(discountPrice == 0)
			{
                if (quantity <= 50)
                {
                    return price;
                }
                else if (quantity <= 100)
                {
                    return price50;
                }
                else
                {
                    if (quantity > 100)
                    {
                        return price100;
                    }

                    return price100;
                }
			}
			else
			{
				return discountPrice;
			}
		}
	}
}
