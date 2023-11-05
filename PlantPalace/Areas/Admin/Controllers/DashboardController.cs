using IronPdf.Extensions.Mvc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;


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
    }
}
