using IronPdf.Extensions.Mvc.Core;
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
		private readonly IRazorViewRenderer _viewRenderService;
        [BindProperty]
        public OrderVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IRazorViewRenderer viewRenderService)
        {
			_unitOfWork = unitOfWork;
            _viewRenderService = viewRenderService;
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
                OrderDetail = _unitOfWork.OrderDetail.GetALL(u => u.OrderHeaderId == OrderId, incluedProperties: "Product"),
                ProductReturnList = _unitOfWork.ProductReturn.GetALL(incluedProperties: "OrderDetail"),
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
        [Authorize(Roles = SD.Role_Admin)]
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
        [Authorize(Roles = SD.Role_Admin)]
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
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult CancelProduct(int orderDetailId)
        {
            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id, incluedProperties: "ApplicationUser");

            var orderDetail = _unitOfWork.OrderDetail.Get(u => u.Id == orderDetailId,incluedProperties:"Product");
            if(orderDetail == null)
            {
                TempData["success"] = "Product Not Found";
                return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
            }
            orderHeader.OrderTotal -= orderDetail.Price;

            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.OrderDetail.Remove(orderDetail);
            var product = _unitOfWork.Product.Get(u => u.Id == orderDetail.ProductId);
            product.Stock += orderDetail.Count;
            _unitOfWork.Product.Update(product);

            _unitOfWork.ApplicationUser.UpdateWallet(orderHeader.ApplicationUser.Id, +orderDetail.Price);
            TempData["success"] = $"{orderDetail.Product.Name} Cancelled Successfully.";
            _unitOfWork.Save();
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize]
        [AutoValidateAntiforgeryToken]
        public IActionResult CancelOrder()
        {


            var orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id,incluedProperties: "ApplicationUser") ;
            var orderDetails = _unitOfWork.OrderDetail.GetALL(u => u.OrderHeaderId == orderHeader.Id);
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
            foreach(var item in orderDetails)
            {
                var product=_unitOfWork.Product.Get(u => u.Id == item.ProductId);
                product.Stock += item.Count;
                _unitOfWork.Product.Update(product);
            }
            _unitOfWork.ApplicationUser.UpdateWallet(orderHeader.ApplicationUser.Id, +orderHeader.OrderTotal);
            _unitOfWork.Save();
            TempData["success"] = "Order Cancelled Successfully.";
            return RedirectToAction(nameof(Details), new { orderId = OrderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize]
        public IActionResult AddReturn(int orderDetailId,string Reason,int orderHeaderId)
        {
            var returnOrder = new ProductReturn()
            {
                Date = DateTime.UtcNow,
                OrderDetailId = orderDetailId,
                ReturnReason = Reason,
                ReturnStatus = SD.ReturnPending,
            };

            _unitOfWork.ProductReturn.Add(returnOrder);
            _unitOfWork.Save();
            TempData["success"] = "Waiting for Seller approval";
            return RedirectToAction(nameof(Details), new { orderId = orderHeaderId });


        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangeReturnStatus(string status,int ProductRetunId, string userId)
        {
            var returnModel = _unitOfWork.ProductReturn.Get(u => u.Id == ProductRetunId,incluedProperties: "OrderDetail");
            if(returnModel != null)
            {
                if (status != null)
                {
                    if (status == SD.ReturnRejected)
                    {
                        _unitOfWork.ProductReturn.UpdateStatus(ProductRetunId, SD.ReturnRejected);
                    }
                    else
                    {
                        if (status == SD.ReturnPending)
                        {
                            _unitOfWork.ProductReturn.UpdateStatus(ProductRetunId, SD.ReturnPending);

                        }
                        else
                        {
                            _unitOfWork.ProductReturn.UpdateStatus(ProductRetunId, SD.ReturnApproved);
                            _unitOfWork.ApplicationUser.UpdateWallet(userId, +returnModel.OrderDetail.Price);


                        }
                    }
                    _unitOfWork.Save();
                    return Json(new { message = $"request status update as {status} " });
                }
            }
            
            return Json(new { message = "error" });

        }

        [HttpGet]
        public IActionResult ReturnList()
        {
            var returnOrders = _unitOfWork.ProductReturn.GetALL(incluedProperties: "OrderDetail");
            foreach (var returnOrder in returnOrders)
            {
                returnOrder.OrderDetail = _unitOfWork.OrderDetail.Get(u=> u.Id == returnOrder.OrderDetailId,incluedProperties: "OrderHeader,Product");
                returnOrder.OrderDetail.OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == returnOrder.OrderDetail.OrderHeaderId,
                    incluedProperties: "ApplicationUser");
            }
            return View(returnOrders.Reverse());
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Invoice(int id)
        {
            var OrderVM = new OrderVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, incluedProperties: "ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetALL(u => u.OrderHeaderId == id, incluedProperties: "Product")
            };



            ChromePdfRenderer renderer = new ChromePdfRenderer();



            // Render View to PDF document
            PdfDocument pdf = renderer.RenderRazorViewToPdf(_viewRenderService, "Areas/Admin/Views/Order/Invoice.cshtml", OrderVM);
            Response.Headers.Add("Content-Disposition", "inline");

            // Output PDF document
            return File(pdf.BinaryData, "application/pdf", $"Invoice_{'#' + id + '_' + DateTime.Now.ToShortDateString()}.pdf");

        }


        [HttpPost]
        [Authorize(Roles =SD.Role_Admin)]
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
