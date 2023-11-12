using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using PlantPalace.DataAccess.Data;
using PlantPalace.DataAccess.Repository;
using PlantPalace.DataAccess.Repository.IRepository;
using PlantPalace.Models;
using PlantPalace.Models.ViewModels;
using PlantPalace.Utility;
using Stripe;

namespace PlantPalaceWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }


        public IActionResult Index()
        {
            var products= _unitOfWork.Product.GetALL(incluedProperties:"Category").ToList();
            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category
                .GetALL().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),

                Product = new PlantPalace.Models.Product()
            };
            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, string? newfile, string? newfile1, string? newfile2, string? newfile3)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _env.WebRootPath;
                if(newfile != null)
                {
                    
                    string productPath = wwwRootPath + @"\Images\product\";

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        var  oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    productVM.Product.ImageUrl = @"\Images\product\" + newfile;
                }
                if (newfile1 != null)
                {

                    string productPath = wwwRootPath + @"\Images\product\";

                    if (!string.IsNullOrEmpty(productVM.Product.ImageOne))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageOne.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    productVM.Product.ImageOne = @"\Images\product\" + newfile1;
                }
                if (newfile2 != null)
                {

                    string productPath = wwwRootPath + @"\Images\product\";

                    if (!string.IsNullOrEmpty(productVM.Product.ImageTwo))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageTwo.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    productVM.Product.ImageTwo = @"\Images\product\" + newfile2;
                }
                if (newfile3 != null)
                {

                    string productPath = wwwRootPath + @"\Images\product\";

                    if (!string.IsNullOrEmpty(productVM.Product.ImageThree))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageThree.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    productVM.Product.ImageThree = @"\Images\product\" + newfile3;
                }
                if(productVM.Product.SubCategory.IsNullOrEmpty())
                {
                    productVM.Product.SubCategory = "general";
                }

                if (productVM.Product.Id == 0)
                {
                    productVM.Product.AddedDate = DateTime.Now;
                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product " + productVM.Product.Name + " Created Successfully";


                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    TempData["success"] = "Product " + productVM.Product.Name + " Updated Successfully";

                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category
                .GetALL().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult CropAndSave(string FORfilename,IFormFile? file)
        {
            string wwwRootPath = _env.WebRootPath;
            if (file != null)
            {
                string filename = Guid.NewGuid().ToString() + "_"+FORfilename;
                string productPath = wwwRootPath + @"\Images\product\";


                ImageCrop crop = new ImageCrop();
                crop.Crop(Path.Combine(productPath, filename), file);
                
                return Json(new { message = "OK" ,filename = filename.ToString()});
            }

            return Json(new { message = "ERROR" });
        }

        /*        public IActionResult Edit(int? id)
                {
                    if (id == null || id == 0)
                    {
                        return NotFound();
                    }

                    Product? product = _unitOfWork.Product.Get(u => u.Id == id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }
                [HttpPost]
                public IActionResult Edit(Product model)
                {
                    if (ModelState.IsValid)
                    {
                        _unitOfWork.Product.Update(model);
                        _unitOfWork.Save();
                        TempData["success"] = "Product " + model.Name + " Edited Successfully";
                        return RedirectToAction("Index");
                    }
                    return View();
                }*/


        /*        public IActionResult Delete(int? id)
                {
                    if (id == null || id == 0)
                    {
                        return NotFound();
                    }

                    Product? product = _unitOfWork.Product.Get(u => u.Id == id);
                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }


                [HttpPost, ActionName("Delete")]
                public IActionResult DeletePost(int? id)
                {
                    if (id == null || id == 0)
                    {
                        return NotFound();
                    }

                    var product = _unitOfWork.Product.Get(u => u.Id == id);
                    string wwwRootPath = _env.WebRootPath;

                    if (product == null)
                        return NotFound();
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    _unitOfWork.Product.Remove(product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product " + product.Name + " Deleted Successfully";
                    return RedirectToAction("Index");
                }*/


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetALL(incluedProperties: "Category");
            
            return Json(new { data = products });
        }



        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
                return Json(new { success = false, message = "Error while deleting" });


            string wwwRootPath = _env.WebRootPath;

            if (!string.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                var oldImagePath = Path.Combine(wwwRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!string.IsNullOrEmpty(productToBeDeleted.ImageOne))
            {
                var oldImagePath = Path.Combine(wwwRootPath, productToBeDeleted.ImageOne.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!string.IsNullOrEmpty(productToBeDeleted.ImageTwo))
            {
                var oldImagePath = Path.Combine(wwwRootPath, productToBeDeleted.ImageTwo.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
            if (!string.IsNullOrEmpty(productToBeDeleted.ImageThree))
            {
                var oldImagePath = Path.Combine(wwwRootPath, productToBeDeleted.ImageThree.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
