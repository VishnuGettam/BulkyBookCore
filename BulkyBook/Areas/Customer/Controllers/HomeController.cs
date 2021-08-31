using BulkyBook.DataAccess.Repository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitofWork _unitofWork;

        public HomeController(ILogger<HomeController> logger, IUnitofWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        public IActionResult Index()
        {
            var products = _unitofWork.Product.GetAll(includeProperties: "Category,CoverType");

            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var prdFromDB = _unitofWork.Product.GetFirstOrDefault(p => p.Id == Id, includeProperties: "Category,CoverType");

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = new Product(),
                ProductId = prdFromDB.Id
            };

            shoppingCart.Product = prdFromDB;

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart model)
        {
            model.Id = 0;

            if (model.Product != null)
            {
                var prd = _unitofWork.Product.GetFirstOrDefault(p => p.Id == model.Product.Id, includeProperties: "Category,CoverType");
                model.Product = prd;
                model.ProductId = prd.Id;
                ModelState["Product.Title"].Errors.Clear();
                ModelState["Product.Title"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.ISBN"].Errors.Clear();
                ModelState["Product.ISBN"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.Author"].Errors.Clear();
                ModelState["Product.Author"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.ListPrice"].Errors.Clear();
                ModelState["Product.ListPrice"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.Price"].Errors.Clear();
                ModelState["Product.Price"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.Price50"].Errors.Clear();
                ModelState["Product.Price50"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;

                ModelState["Product.Price100"].Errors.Clear();
                ModelState["Product.Price100"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            }

            if (ModelState.IsValid)
            {
                var appUserId = _unitofWork.ApplicationUser.GetFirstOrDefault(a => a.UserName == User.Identity.Name)?.Id;
                model.ApplicationUserId = appUserId;
                ShoppingCart objFromDb = _unitofWork.ShoppingCart
                                                     .GetFirstOrDefault(s => s.ProductId == model.ProductId && s.ApplicationUserId == model.ApplicationUserId, includeProperties: "Product");

                if (objFromDb == null)
                {
                    //no products added by user to cart
                    _unitofWork.ShoppingCart.Add(model);
                }
                else
                {
                    objFromDb.Count += model.Count;
                    _unitofWork.ShoppingCart.Update(objFromDb);
                }
                _unitofWork.Save();

                return RedirectToAction("Index");
            }
            else
            {
                var prdFromDB = _unitofWork.Product.GetFirstOrDefault(p => p.Id == model.Product.Id, includeProperties: "Category,CoverType");

                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    Product = prdFromDB,
                    ProductId = prdFromDB.Id
                };

                return View(shoppingCart);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}