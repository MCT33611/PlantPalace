using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;
using Stripe;
using System.Diagnostics;
using System.Security.Claims;

namespace PlantPalaceWeb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
	public class OrderController : Controller
	{	
		private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
			_unitOfWork = unitOfWork;
        }
        public IActionResult Index()
		{
			return View();
		}

        public IActionResult Details(int OrderId)
        {
            OrderVM = new()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderId, incluedProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetALL(u => u.OrderHeaderId == OrderId, incluedProperties: "Product")
            };
            return View(OrderVM);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles ="Admin")]

        public IActionResult StartProcessing()
        {
            

            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id,SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["Success"] = "Order Status Updated Successfully";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public IActionResult SetASPaid()
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.PaymentStatus = SD.PaymentStatusApproved;

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Payment completed Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        public IActionResult COD_PaymentConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id);
            _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusPending);
            _unitOfWork.Save();

            return View(id);
        }

        
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [AutoValidateAntiforgeryToken]
        public IActionResult ShipOrder()
        {


            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            if (orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
            }

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Save();
            TempData["Success"] = "Order Shipped Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        [AutoValidateAntiforgeryToken]
        public IActionResult CancelOrder()
        {


            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            if (orderHeader.PaymentStatus == SD.PaymentStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id,SD.StatusCancelled,SD.StatusCancelled);

            }
            else
            {
                _unitOfWork.OrderHeader.UpdateStatus(orderHeader.Id, SD.StatusCancelled, SD.StatusRefunded);

            }
            _unitOfWork.Save();
            TempData["Success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }



        [HttpPost]
        [Authorize(Roles ="Admin")]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpdateOrderDetail()
        {
            var OrderHeaderDB = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id,tracked:false);
            OrderHeaderDB.Name = OrderVM.OrderHeader.Name;
            OrderHeaderDB.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            OrderHeaderDB.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            OrderHeaderDB.City = OrderVM.OrderHeader.City;
            OrderHeaderDB.State = OrderVM.OrderHeader.State;
            OrderHeaderDB.PostalCode = OrderVM.OrderHeader.PostalCode;
            if(OrderVM.OrderHeader.Carrier != null)
            {
                OrderHeaderDB.Carrier = OrderVM.OrderHeader.Carrier;

            }
            if (OrderVM.OrderHeader.TrackingNumber != null)
            {
                OrderHeaderDB.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;

            }

            _unitOfWork.OrderHeader.Update(OrderHeaderDB);
            _unitOfWork.Save();
            TempData["Success"] = "Order Details Updated Successfully";
            return RedirectToAction("Details", "Order", new { orderId = OrderHeaderDB.Id });
        }




        #region API CALLS
        [HttpGet]
        [Authorize]

        public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> orderHeaders;

            if(User.IsInRole(SD.Role_Admin))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetALL(incluedProperties: "ApplicationUser");

            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                orderHeaders = _unitOfWork.OrderHeader.GetALL(u => u.ApplicationUserId == claim.Value, incluedProperties: "ApplicationUser");


            }


            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                default:
                    break;

            }



            return Json(new { data = orderHeaders });

		}
		#endregion
	}
}
