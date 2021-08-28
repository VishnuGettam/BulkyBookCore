using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IUnitofWork _unitofWork;

        public CoverTypeController(ApplicationDbContext db, IUnitofWork unitofWork)
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
                var dapper = new Dapper.DynamicParameters();
                dapper.Add("Id", Id);

                var coverType = _unitofWork.StoredProcedureCall.OneRecord<CoverType>(SD.CoverTypeSP.GetCoverType.ToString(), dapper);
                return View(coverType);
            }

            return View();
        }

        [HttpPost]
        public IActionResult UpSert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                if (coverType.Id == 0)
                {
                    var dapper = new Dapper.DynamicParameters();
                    dapper.Add("name", coverType.Name);
                    _unitofWork.StoredProcedureCall.Execute(SD.CoverTypeSP.AddCoverType.ToString(), dapper);
                    _unitofWork.Save();
                }
                else
                {
                    var dapper = new Dapper.DynamicParameters();
                    dapper.Add("id", coverType.Id);
                    dapper.Add("name", coverType.Name);
                    _unitofWork.StoredProcedureCall.Execute(SD.CoverTypeSP.UpdateCoverType.ToString(), dapper);
                    _unitofWork.Save();
                }
                return RedirectToAction("Index", "CoverType", new { area = "Admin" });
            }

            return View();
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var coverTypes = _unitofWork.StoredProcedureCall.List<CoverType>(SD.CoverTypeSP.GetAllCoverType.ToString()).ToList();

            return Json(new { data = coverTypes });
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var dapper = new Dapper.DynamicParameters();
            dapper.Add("id", Id);

            var objFromDB = _unitofWork.StoredProcedureCall.OneRecord<CoverType>(SD.CoverTypeSP.GetCoverType.ToString(), dapper);

            if (objFromDB != null)
            {
                _unitofWork.StoredProcedureCall.Execute(SD.CoverTypeSP.DeleteCoverType.ToString(), dapper);
                _unitofWork.Save();
                return Json(new { Result = "Success", Message = " Deleted Succesfully!.. " });
            }
            else
            {
                return Json(new { Result = "Error", Message = "Error occured while deleting !..." });
            }
        }

        #endregion API Calls
    }
}