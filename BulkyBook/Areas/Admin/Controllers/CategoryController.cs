using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Migrations;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitofWork _unitofWork;

        public CategoryController(ApplicationDbContext db, IUnitofWork unitofWork)
        {
            _db = db;
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpSert(int? Id)
        {
            if (Id != null)
            {
                var category = _unitofWork.Category.Get(Id.Value);
                return View(category);
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpSert(Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.Id == 0)
                {
                    _unitofWork.Category.Add(category);
                    _unitofWork.Save();
                }
                else
                {
                    _unitofWork.Category.UpdateCategory(category);
                }
                return RedirectToAction("Index", "Category", new { area = "Admin" });
            }

            return View();
        }

        #region APICalls

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _unitofWork.Category.GetAll();
            return Json(new { data = categories });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var objFromDB = _unitofWork.Category.Get(Id);

            if (objFromDB != null)
            {
                _unitofWork.Category.Remove(objFromDB);
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