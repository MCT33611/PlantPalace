
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;

namespace PlantPalace.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class OfferController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public OfferController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;

		}
		public IActionResult Index()
		{
			var Offers = _unitOfWork.Offer.GetALL();
			return View(Offers);
		}

		public IActionResult Upsert(int? id)
		{
			var vm = new OfferVM();
			vm.Offer = new Offer();
			if(id != null && id != 0)
			{
				vm.Offer = _unitOfWork.Offer.Get(u => u.Id == id);

			}
			vm.Products = _unitOfWork.Product.GetALL().ToList(); ;
			return View(vm);
		}
		[HttpPost]
		public IActionResult Upsert(OfferVM model, string? newfile)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;

			if (newfile != null)
			{


				if (!string.IsNullOrEmpty(model.Offer.ImageUrl))
				{
					var oldImagePath = Path.Combine(wwwRootPath, model.Offer.ImageUrl.TrimStart('\\'));

					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}
				}
				model.Offer.AddedDate = DateTime.UtcNow;
				model.Offer.ImageUrl = @"\Images\Offers\" + newfile;
			}
			if(model.Offer.OfferName == null)
			{
				ModelState.AddModelError("OfferName", "Plaese Enter the Offer Name");
			}
			else
			{
				model.Offer.OfferUrl = "/customer/home/ProductList?offer="+model.Offer.OfferName;

            }
			
			if (ModelState.IsValid)
			{
				var product = _unitOfWork.Product.Get(u => u.Id == model.Offer.ProductId);
				product.DiscountPrice = model.Offer.OfferPrice;
				_unitOfWork.Product.Update(product);
				if(model.Offer.Id != 0 && model.Offer.Id != null)
				{
                    _unitOfWork.Offer.Update(model.Offer);
                    TempData["success"] = "Offer Edited Successfully";

                }
				else
				{
                    _unitOfWork.Offer.Add(model.Offer);
                    TempData["success"] = "Offer Created Successfully";
                }
                

                _unitOfWork.Save();
				return RedirectToAction("Index");
			}
			ModelState.AddModelError("Name", "name is required");
			return View();
		}

		[HttpPost]
		public IActionResult CropAndSave(string FORfilename, IFormFile? file)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			if (file != null)
			{
				string filename = Guid.NewGuid().ToString() + "_" + FORfilename;
				string productPath = wwwRootPath + @"\Images\Offers\";


				ImageCrop crop = new ImageCrop();
				crop.CropBanner(Path.Combine(productPath, filename), file);

				return Json(new { message = "OK", filename = filename.ToString() });
			}

			return Json(new { message = "ERROR" });
		}



		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Offer? Offer = _unitOfWork.Offer.Get(u => u.Id == id,incluedProperties: "Product");
			if (Offer == null)
			{
				return NotFound();
			}
			return View(Offer);
		}


		[HttpDelete, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var Offer = _unitOfWork.Offer.Get(u => u.Id == id);

			if (Offer == null)
				return NotFound();

			var product = _unitOfWork.Product.Get(u => u.Id == Offer.ProductId);
			product.DiscountPrice = 0;
			_unitOfWork.Product.Update(product);
			_unitOfWork.Offer.Remove(Offer);
			_unitOfWork.Save();
			//TempData["success"] = "Offer Deleted Successfully";
			return Json(new {message = "Offer Deleted Successfully" });
		}
	}
}
