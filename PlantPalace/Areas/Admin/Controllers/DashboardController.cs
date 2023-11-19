using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace PlantPalace.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRazorViewRenderer _viewRenderService;



        public DashboardController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor, IRazorViewRenderer viewRenderService)
        {
             _unitOfWork = unitOfWork;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            DashboardVM vm = new()
            {
                orders = _unitOfWork.OrderHeader.GetALL(incluedProperties: "ApplicationUser"),

                ordersDetail = _unitOfWork.OrderDetail.GetALL(incluedProperties: "OrderHeader,Product"),


                products = _unitOfWork.Product.GetALL(incluedProperties: "Category").ToList(),

                users = _unitOfWork.ApplicationUser.GetALL().ToList(),

                productReviews = _unitOfWork.ProductReview.GetALL().ToList(),

            };


            foreach (var user in vm.users)
            {

                user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
            }

            return View(vm);
        }

        // PM > Install-Package IronPdf.Extensions.Mvc.Core

        public async Task<IActionResult> SalesReport(string? filter)
        {
            var orders = _unitOfWork.OrderHeader.GetALL(incluedProperties: "ApplicationUser");
            if(filter != null)
            {
                if(filter == "today")
                {
                    orders = orders.Where(o => o.OrderDate.Date == DateTime.Now.Date);
                }
                else if (filter == "month")
                {
                    orders = orders.Where(o => o.OrderDate.Month == DateTime.Now.Month);
                }
                else if (filter == "year")
                {
                    orders = orders.Where(o => o.OrderDate.Year == DateTime.Now.Year);
                }
                else
                {
                    orders = _unitOfWork.OrderHeader.GetALL( incluedProperties: "ApplicationUser");

                }

            }

            if (_httpContextAccessor.HttpContext.Request.Method == HttpMethod.Post.Method)
            {
                ChromePdfRenderer renderer = new ChromePdfRenderer();

                

                // Render View to PDF document
                PdfDocument pdf = renderer.RenderRazorViewToPdf(_viewRenderService, "Areas/Admin/Views/DashBoard/SalesReport.cshtml", orders);
                Response.Headers.Add("Content-Disposition", "inline");

                // Output PDF document
                return File(pdf.BinaryData, "application/pdf", $"SalesReport_this_{filter.ToUpper()+'_'+DateTime.Now.ToShortDateString()}.pdf");
            }
            return View(orders);
        }



        public IActionResult ChartOfAdminData()
        {
            try
            {
                var Dates = new List<string>();
                var currentDate = DateTime.UtcNow;

                for (int i = 0; i < 7; i++)
                {
                    Dates.Add(currentDate.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
                    currentDate = currentDate.AddDays(1);
                }

                var Customers = new List<string>(); // Change to string
                var Sales = new List<string>(); // Change to string
                var Revenue = new List<string>(); // Change to string

                var totalUsers = _unitOfWork.ApplicationUser.GetALL();
                var totalSales = _unitOfWork.OrderDetail.GetALL(incluedProperties: "OrderHeader,Product").ToList();
                var totalRevenue = _unitOfWork.OrderHeader.GetALL(incluedProperties: "ApplicationUser").ToList();

                for (int i = 0; i < 7; i++)
                {
                    var date = DateTime.ParseExact(Dates[i], "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

                    // Customers
                    Customers.Add(totalUsers.Count(u => u.joinDate.Month == date.Month && u.joinDate.Date == date.Date).ToString());

                    // Sales
                    var dailySales = totalSales
                        .Where(u => u.OrderHeader.OrderDate.Date == date.Date && u.OrderHeader.OrderDate.Month == date.Month)
                        .ToList();

                    Sales.Add((dailySales.Count * dailySales.Sum(u => u.Count)).ToString());

                    // Revenue
                    Revenue.Add(((int)totalRevenue
                        .Where(u => u.OrderDate.Date == date.Date && u.OrderDate.Month == date.Month)
                        .Sum(u => u.OrderTotal)).ToString());
                }

                var data = new { Dates, Customers, Sales, Revenue };
                return Json(data);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine(ex.Message);
                return BadRequest("Error processing data");
            }
        }



    }
}
