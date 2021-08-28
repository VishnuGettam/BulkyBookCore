using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitofWork _unitofWork;
        private readonly IWebHostEnvironment _env;

        public ProductController(ApplicationDbContext db, IUnitofWork unitofWork, IWebHostEnvironment hostenv)
        {
            _db = db;
            _unitofWork = unitofWork;
            _env = hostenv;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpSert(int? Id)
        {
            ViewBag.Categories = new SelectList(_unitofWork.Category.GetAll(), "Id", "Name");
            ViewBag.CoverTypes = new SelectList(_unitofWork.CoverType.GetAll(), "Id", "Name");
            ViewBag.Type = "Create";


            if (Id != null)
            {
                ViewBag.Type = "Update";
                var product = _unitofWork.Product.Get(Id.Value);

                if (product.ImageURL != null)
                {
                    byte[] img = Convert.FromBase64String(product.ImageURL);
                    product.ImageURL = "data:image/png;base64," + Convert.ToBase64String(img);
                }
                return View(product);
            }

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult UpSert(Product product, IFormFile imageURL)
        {
            ViewBag.Categories = new SelectList(_unitofWork.Category.GetAll(), "Id", "Name");
            ViewBag.CoverTypes = new SelectList(_unitofWork.CoverType.GetAll(), "Id", "Name");

            if (product.Price != 0)
            {
                product.Price50 = 5 * product.Price;
                product.Price100 = 10 * product.Price;

                ModelState["Price50"].Errors.Clear();
                ModelState["Price50"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
                ModelState["Price100"].Errors.Clear();
                ModelState["Price100"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            ////image
            using (var ms = new MemoryStream())
            {
                imageURL.CopyTo(ms);
                var fileBytes = ms.ToArray();
                var base64Image = Convert.ToBase64String(fileBytes);
                product.ImageURL = base64Image;
            }

            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    _unitofWork.Product.Add(product);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Product.Update(product);
                }
                return RedirectToAction("Index", "Product", new { area = "Admin" });
            }

            return View();
        }

        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitofWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var objFromDB = _unitofWork.Product.Get(Id);

            if (objFromDB != null)
            {
                _unitofWork.Product.Remove(objFromDB);
                _unitofWork.Save();
                return Json(new { Result = "Success", Message = " Deleted Succesfully!.. " });
            }
            else
            {
                return Json(new { Result = "Error", Message = "Error occured while deleting !..." });
            }
        }

        #endregion APICalls
    }
}