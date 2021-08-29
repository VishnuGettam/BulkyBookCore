using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin" + "," + "Employee")]
    public class UserController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        private readonly ApplicationDbContext _db;

        public UserController(IUnitofWork unitofWork, ApplicationDbContext db)
        {
            _unitofWork = unitofWork;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetUsers()
        {
            var userList = _unitofWork.ApplicationUser.GetAll(includeProperties: "Company");
            var roles = _db.Roles.ToList();
            var userRoles = _db.UserRoles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRoles.FirstOrDefault(r => r.UserId == user.Id).RoleId;
                var roleName = roles.FirstOrDefault(r => r.Id == roleId).Name;
                user.Role = roleName;

                if (user.Company == null)
                {
                    user.Company = new Models.ViewModels.Company()
                    {
                        Name = ""
                    };
                }
            }

            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnLock(string userId, string status)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(r => r.Id == userId);

            if (user != null)
            {
                if (status == "lock")
                {
                    user.LockoutEnd = DateTime.Now.AddDays(3);
                }
                else
                {
                    user.LockoutEnd = DateTime.Now;
                }

                _db.Entry<ApplicationUser>(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
            }
            return Json(new { result = (status == "lock") ? " Locked Succesfully!.. " : "Unlocked Succesfullu" });
        }

        #endregion API Calls
    }
}