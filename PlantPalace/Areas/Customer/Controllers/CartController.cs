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
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        private readonly IEmailSender _emailSender;
        public CartController(IUnitOfWork unitOfWork,IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
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
                cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
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
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == claim.Value);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAdderss;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Quantity);
            }
            return View(ShoppingCartVM);
        }


        [ActionName("Summary")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost()
        {
			bool isCheckBoxChecked = Request.Form["IsChecked"] == "on";
			if (!isCheckBoxChecked)
            {
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product");

				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                ShoppingCartVM.OrderHeader.PaymentMethod = SD.PaymentMethodOnline;
                ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
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

				var domain = "https://localhost:7253/";
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
							Currency = "usd",
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
           else
            {
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

				ShoppingCartVM.ListCart = _unitOfWork.ShoppingCart.GetALL(u => u.userId == claim.Value, incluedProperties: "Product");

				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                ShoppingCartVM.OrderHeader.PaymentMethod = SD.PaymentMethodCOD;
				ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
				ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;

				foreach (var cart in ShoppingCartVM.ListCart)
				{
					cart.Price = GetPriceBasedOnQuantity(cart.Quantity, cart.Product.Price, cart.Product.Price50, cart.Product.Price100);
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

				

				_unitOfWork.OrderHeader.Update(ShoppingCartVM.OrderHeader);
				_unitOfWork.Save();

				return RedirectToAction("OrderConfirmationOffline", "Cart", new {id = ShoppingCartVM.OrderHeader.Id});
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

            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetALL(u => u.userId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();
            return View(id);
        }

		public IActionResult OrderConfirmationOffline(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id,incluedProperties: "ApplicationUser");
            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusPending);
		    _unitOfWork.Save();
            

			return RedirectToAction("OrderConfirmation", "Cart", new {id = id });

		}

		public IActionResult OrderConfirmationOnline(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);

            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId); 

            if(session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusDelayedPayment);
                _unitOfWork.Save();
            }
/*            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetALL(u => u.userId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();*/

			return RedirectToAction("OrderConfirmation", "Cart", new {id = id});

		}

		public IActionResult Plus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.QuantityIncrement(cart,1);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            if(cart.Quantity <= 1)
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
        private double GetPriceBasedOnQuantity(double  quantity,double price,double price50,double price100)
        {
            if(quantity<= 50)
            {
                return price;
            }
            else if(quantity <= 100)
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
    }
}
