using BulkyBook.DataAccess.Repository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitofWork _unitofWork;
        public TwilioSettings twilioSettings { get; set; }

        [BindProperty]
        public ShoppingCartViewModel _shoppingCartViewModel { get; set; }

        public ShoppingCartController(IUnitofWork unitofWork, IOptions<TwilioSettings> options)
        {
            _unitofWork = unitofWork;
            twilioSettings = options.Value;
        }

        public IActionResult Index()
        {
            var appUSer = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            ShoppingCartViewModel shpCartViewModel = new ShoppingCartViewModel()
            {
                OrderHeader = new OrderHeader()
                {
                    ApplicationUser = _unitofWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == User.Identity.Name, includeProperties: "Company"),
                    ApplicationUserId = appUSer.Id
                },
                ListCart = _unitofWork.ShoppingCart
                .GetAll(s => s.ApplicationUserId == appUSer.Id, includeProperties: "Product").ToList()
            };

            shpCartViewModel.OrderHeader.OrderTotal = 0;

            foreach (var shpProduct in shpCartViewModel.ListCart)
            {
                shpProduct.Price = SD.GetProductPriceonQuantity(shpProduct.Count,
                                    shpProduct.Product.Price, shpProduct.Product.Price50,
                                    shpProduct.Product.Price100);

                shpCartViewModel.OrderHeader.OrderTotal += (shpProduct.Price * shpProduct.Count);
                shpProduct.Product.Description = SD.ConvertToRawHtml(shpProduct.Product.Description);

                if (shpProduct.Product.Description.Length > 100)
                {
                    shpProduct.Product.Description = shpProduct.Product.Description.Substring(0, 99) + "....";
                }
            }

            return View(shpCartViewModel);
        }

        public IActionResult UpdateProductCount(int id)
        {
            int incrementalCount = 1;

            var appUserId = _unitofWork.ApplicationUser.GetFirstOrDefault(a => a.UserName == User.Identity.Name).Id;
            var shpCartData = _unitofWork.ShoppingCart.GetFirstOrDefault(s => s.ProductId == id && s.ApplicationUserId == appUserId);
            shpCartData.Count += incrementalCount;

            _unitofWork.ShoppingCart.Update(shpCartData);
            _unitofWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult ReduceProductCount(int id)
        {
            int decrementalCount = -1;

            var appUserId = _unitofWork.ApplicationUser.GetFirstOrDefault(a => a.UserName == User.Identity.Name).Id;
            var shpCartData = _unitofWork.ShoppingCart.GetFirstOrDefault(s => s.ProductId == id && s.ApplicationUserId == appUserId);
            shpCartData.Count += decrementalCount;

            if (shpCartData.Count == 0)
            {
                return RedirectToAction("RemoveFromCart", new { id = id });
            }

            _unitofWork.ShoppingCart.Update(shpCartData);
            _unitofWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var appUserId = _unitofWork.ApplicationUser.GetFirstOrDefault(a => a.UserName == User.Identity.Name).Id;
            var shpCartData = _unitofWork.ShoppingCart.GetFirstOrDefault(s => s.ProductId == id && s.ApplicationUserId == appUserId);

            _unitofWork.ShoppingCart.Remove(shpCartData);
            _unitofWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Summary()
        {
            var appUSer = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
            ShoppingCartViewModel shpCartViewModel = new ShoppingCartViewModel()
            {
                OrderHeader = new OrderHeader()
                {
                    ApplicationUser = _unitofWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == User.Identity.Name, includeProperties: "Company"),
                    ApplicationUserId = appUSer.Id
                },
                ListCart = _unitofWork.ShoppingCart
                .GetAll(s => s.ApplicationUserId == appUSer.Id, includeProperties: "Product").ToList()
            };

            shpCartViewModel.OrderHeader.OrderTotal = 0;

            foreach (var shpProduct in shpCartViewModel.ListCart)
            {
                shpProduct.Price = SD.GetProductPriceonQuantity(shpProduct.Count,
                                    shpProduct.Product.Price, shpProduct.Product.Price50,
                                    shpProduct.Product.Price100);

                shpCartViewModel.OrderHeader.OrderTotal += (shpProduct.Price * shpProduct.Count);
            }

            shpCartViewModel.OrderHeader.Name = appUSer.Name;
            shpCartViewModel.OrderHeader.PhoneNumber = appUSer.PhoneNumber;
            shpCartViewModel.OrderHeader.StreetAddress = appUSer.StreetAddress;
            shpCartViewModel.OrderHeader.City = appUSer.City;
            shpCartViewModel.OrderHeader.State = appUSer.State;
            shpCartViewModel.OrderHeader.PostalCode = appUSer.PostalCode;

            return View(shpCartViewModel);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult Summary_Post()
        {
            var shp = _shoppingCartViewModel;

            if (ModelState.IsValid)
            {
                var appUSer = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == User.Identity.Name);
                ShoppingCartViewModel shpCartViewModel = new ShoppingCartViewModel()
                {
                    OrderHeader = new OrderHeader()
                    {
                        ApplicationUser = _unitofWork.ApplicationUser
                        .GetFirstOrDefault(u => u.UserName == User.Identity.Name, includeProperties: "Company"),
                        ApplicationUserId = appUSer.Id
                    },
                    ListCart = _unitofWork.ShoppingCart
                    .GetAll(s => s.ApplicationUserId == appUSer.Id, includeProperties: "Product").ToList()
                };

                shpCartViewModel.OrderHeader.OrderTotal = 0;

                foreach (var shpProduct in shpCartViewModel.ListCart)
                {
                    shpProduct.Price = SD.GetProductPriceonQuantity(shpProduct.Count,
                                        shpProduct.Product.Price, shpProduct.Product.Price50,
                                        shpProduct.Product.Price100);

                    shpCartViewModel.OrderHeader.OrderTotal += (shpProduct.Price * shpProduct.Count);
                }

                shpCartViewModel.OrderHeader.PaymentStatus = "Pending";
                shpCartViewModel.OrderHeader.OrderStatus = "Pending";
                shpCartViewModel.OrderHeader.OrderDate = DateTime.Now;
                shpCartViewModel.OrderHeader.Name = appUSer.Name;
                shpCartViewModel.OrderHeader.PhoneNumber = appUSer.PhoneNumber;
                shpCartViewModel.OrderHeader.StreetAddress = appUSer.StreetAddress;
                shpCartViewModel.OrderHeader.City = appUSer.City;
                shpCartViewModel.OrderHeader.State = appUSer.State;
                shpCartViewModel.OrderHeader.PostalCode = appUSer.PostalCode;

                _unitofWork.OrderHeader.Add(shpCartViewModel.OrderHeader);
                _unitofWork.Save();

                foreach (var shpProduct in shpCartViewModel.ListCart)
                {
                    OrderDetails orderDetails = new OrderDetails()
                    {
                        ProductId = shpProduct.ProductId,
                        OrderId = shpCartViewModel.OrderHeader.Id,
                        Price = shpProduct.Price,
                        Count = shpProduct.Count
                    };
                    _unitofWork.OrderDetails.Add(orderDetails);
                }

                _unitofWork.ShoppingCart.RemoveRange(shpCartViewModel.ListCart);
                _unitofWork.Save();

                return RedirectToAction("OrderConfirmation", new { orderHeaderId = shpCartViewModel.OrderHeader.Id });
            }
            else
            {
                return RedirectToAction("Summary");
            }
        }

        public IActionResult OrderConfirmation(int orderHeaderId)
        {
            TwilioClient.Init(twilioSettings.AccountSid, twilioSettings.Authtoken);

            try
            {
                var message = MessageResource.Create(
                    body: "Order Placed on Bulky Book with OrderID : " + orderHeaderId,
                    from: new Twilio.Types.PhoneNumber(twilioSettings.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber("+918073863689"));
            }
            catch (Exception ex)
            {
            }
            return View(orderHeaderId);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = _unitofWork.ApplicationUser.GetFirstOrDefault(u => u.UserName == User.Identity.Name)?.Id;
            var cartfromDB = _unitofWork.ShoppingCart.GetAll(s => s.ApplicationUserId == userId, includeProperties: "Product");

            return Json(new { data = cartfromDB });
        }

        #endregion API Calls
    }
}