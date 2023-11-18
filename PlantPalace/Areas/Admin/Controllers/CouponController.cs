using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;

namespace PlantPalace.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CouponController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CouponController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}
		public IActionResult Index()
		{

			CouponVM vm = new()
			{
				Coupons = _unitOfWork.Coupon.GetALL().ToList(),
				CouponsData = _unitOfWork.CouponsData.GetALL().ToList(),
			};
			return View(vm);
		}

		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Coupon model)
		{


			if (ModelState.IsValid)
			{
				model.PublishDate = DateTime.UtcNow;
				_unitOfWork.Coupon.Add(model);
				_unitOfWork.Save();
				TempData["success"] = "Coupon Created Successfully";
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("Name", "name is required");
			TempData["error"] = "Something went wrong ";

			return View();
		}




		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Coupon? coupon = _unitOfWork.Coupon.Get(u => u.Id == id);
			if (coupon == null)
			{
				return NotFound();
			}
			return View(coupon);
		}
		[HttpPost]
		public IActionResult Edit(Coupon model)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Coupon.Update(model);
				_unitOfWork.Save();
				TempData["success"] = "Coupen Edited Successfully";
				return RedirectToAction("Index");
			}
			return View();
		}


		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Coupon? coupon = _unitOfWork.Coupon.Get(u => u.Id == id);
			if (coupon == null)
			{
				return NotFound();
			}
			return View(coupon);
		}


		[HttpDelete, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var coupon = _unitOfWork.Coupon.Get(u => u.Id == id);

			if (coupon == null)
				return NotFound();


			_unitOfWork.Coupon.Remove(coupon);
			_unitOfWork.Save();
			TempData["success"] = "Coupon Deleted Successfully";
			return RedirectToAction("Index");
		}
	}
}