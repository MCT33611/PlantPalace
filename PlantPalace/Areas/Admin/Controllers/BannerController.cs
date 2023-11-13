using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;

namespace PlantPalace.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(SD.Role_Admin)]
    public class BannerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BannerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var banners = _unitOfWork.Banner.GetALL();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Banner model,string newfile)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (newfile != null)
            {

                string productPath = wwwRootPath + @"\Images\product\";

                if (!string.IsNullOrEmpty(model.ImageUrl))
                {
                    var oldImagePath = Path.Combine(wwwRootPath, model.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                model.AddedDate = DateTime.UtcNow;
                model.ImageUrl = @"\Images\banners\" + newfile;
            }
            if (ModelState.IsValid)
            {
                
                _unitOfWork.Banner.Add(model);
                _unitOfWork.Save();
                TempData["success"] = "Banner Created Successfully";
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
                string productPath = wwwRootPath + @"\Images\banners\";


                ImageCrop crop = new ImageCrop();
                crop.CropBanner(Path.Combine(productPath, filename), file);

                return Json(new { message = "OK", filename = filename.ToString() });
            }

            return Json(new { message = "ERROR" });
        }


        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Banner? banner = _unitOfWork.Banner.Get(u => u.Id == id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }
        [HttpPost]
        public IActionResult Edit(Banner model)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Banner.Update(model);
                _unitOfWork.Save();
                TempData["success"] = "Banner Edited Successfully";
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

            Banner? banner = _unitOfWork.Banner.Get(u => u.Id == id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }


        [HttpDelete, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var banner = _unitOfWork.Banner.Get(u => u.Id == id);

            if (banner == null)
                return NotFound();


            _unitOfWork.Banner.Remove(banner);
            _unitOfWork.Save();
            TempData["success"] = "Banner Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
