using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitofWork _unitofWork;

        public CompanyController(ApplicationDbContext db, IUnitofWork unitofWork)
        {
            _db = db;
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? Id)
        {
            ViewBag.Type = "Create";
            if (Id != null)
            {
                ViewBag.Type = "Update";
                var company = _unitofWork.Company.Get(Id.Value);
                return View(company);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                //Update
                if (company.Id != 0)
                {
                    _unitofWork.Company.Update(company);
                }
                else
                {
                    _unitofWork.Company.Add(company);
                }
                _unitofWork.Save();
                return RedirectToAction("Index", "Company", new { area = "Admin" });
            }

            return View();
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitofWork.Company.GetAll();

            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult DeleteCompany(int Id)
        {
            var objFromDB = _unitofWork.Company.Get(Id);

            if (objFromDB != null)
            {
                _unitofWork.Company.Remove(Id);
                _unitofWork.Save();
                return Json(new { result = true, message = "Deleted Succesfully" });
            }
            else
            {
                return Json(new { result = false, message = "Error while deleting ..." });
            }
        }

        #endregion API Calls
    }
}