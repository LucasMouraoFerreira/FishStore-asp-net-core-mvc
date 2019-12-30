using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FishStore.Data;
using FishStore.Models;
using FishStore.Models.ViewModels;
using FishStore.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace FishStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsCart detailCart { get; set; }

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        //INDEX - GET
        public async Task<IActionResult> Index()
        {


            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0.0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }


            double weigth = 0.0;
            double volume = 0.0;

            foreach (var list in detailCart.listCart)
            {
                list.StoreItem = await _db.StoreItem.FirstOrDefaultAsync(m => m.Id == list.StoreItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.StoreItem.Price * list.Count);
                weigth += (list.StoreItem.Weight * list.Count);
                volume += (list.StoreItem.Volume * list.Count);
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssUserName) != null)
            {
                detailCart.OrderHeader.Name = HttpContext.Session.GetString(SD.ssUserName);
            }
            else
            {
                detailCart.OrderHeader.Name = applicationUser.Name;
            }
            if (HttpContext.Session.GetString(SD.ssUserAddress) != null)
            {
                detailCart.OrderHeader.Address = HttpContext.Session.GetString(SD.ssUserAddress);
            }
            else
            {
                detailCart.OrderHeader.Address = applicationUser.StreetAddress;
            }
            if (HttpContext.Session.GetString(SD.ssUserCity) != null)
            {
                detailCart.OrderHeader.City = HttpContext.Session.GetString(SD.ssUserCity);
            }
            else
            {
                detailCart.OrderHeader.City = applicationUser.City;
            }
            if (HttpContext.Session.GetString(SD.ssUserState) != null)
            {
                detailCart.OrderHeader.State = HttpContext.Session.GetString(SD.ssUserState);
            }
            else
            {
                detailCart.OrderHeader.State = applicationUser.State;
            }

            if (HttpContext.Session.GetString(SD.ssPostalCode) != null)
            {
                detailCart.OrderHeader.PostalCode = HttpContext.Session.GetString(SD.ssPostalCode);
                string[] precoPrazo = await SD.GetPriceAndTimePostalServiceAsync(detailCart.OrderHeader.PostalCode, weigth, volume);
                detailCart.OrderHeader.PostalPrice = Convert.ToDouble(precoPrazo[0]);
                detailCart.OrderHeader.PostalTime = Convert.ToDouble(precoPrazo[1]);
                detailCart.OrderHeader.OrderTotal += detailCart.OrderHeader.PostalPrice;
                HttpContext.Session.SetInt32("ssPostalPrice", Convert.ToInt32(detailCart.OrderHeader.PostalPrice * 100));
            }
            else
            {
                detailCart.OrderHeader.PostalCode = applicationUser.PostalCode;
                string[] precoPrazo = await SD.GetPriceAndTimePostalServiceAsync(detailCart.OrderHeader.PostalCode, weigth, volume);
                detailCart.OrderHeader.PostalPrice = Convert.ToDouble(precoPrazo[0]);
                detailCart.OrderHeader.PostalTime = Convert.ToDouble(precoPrazo[1]);
                detailCart.OrderHeader.OrderTotal += detailCart.OrderHeader.PostalPrice;
                HttpContext.Session.SetInt32("ssPostalPrice", Convert.ToInt32(detailCart.OrderHeader.PostalPrice * 100));
            }


            return View(detailCart);
        }

        //SUMMARY - GET
        public async Task<IActionResult> Summary()
        {

            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0.0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationUser = await _db.ApplicationUser.Where(c => c.Id == claim.Value).FirstOrDefaultAsync();
            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if (cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            double weigth = 0.0;
            double volume = 0.0;

            foreach (var list in detailCart.listCart)
            {
                list.StoreItem = await _db.StoreItem.FirstOrDefaultAsync(m => m.Id == list.StoreItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.StoreItem.Price * list.Count);
                weigth += (list.StoreItem.Weight * list.Count);
                volume += (list.StoreItem.Volume * list.Count);
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssUserName) != null)
            {
                detailCart.OrderHeader.Name = HttpContext.Session.GetString(SD.ssUserName);
            }
            else
            {
                detailCart.OrderHeader.Name = applicationUser.Name;
            }
            if (HttpContext.Session.GetString(SD.ssUserAddress) != null)
            {
                detailCart.OrderHeader.Address = HttpContext.Session.GetString(SD.ssUserAddress);
            }
            else
            {
                detailCart.OrderHeader.Address = applicationUser.StreetAddress;
            }
            if (HttpContext.Session.GetString(SD.ssUserCity) != null)
            {
                detailCart.OrderHeader.City = HttpContext.Session.GetString(SD.ssUserCity);
            }
            else
            {
                detailCart.OrderHeader.City = applicationUser.City;
            }
            if (HttpContext.Session.GetString(SD.ssUserState) != null)
            {
                detailCart.OrderHeader.State = HttpContext.Session.GetString(SD.ssUserState);
            }
            else
            {
                detailCart.OrderHeader.State = applicationUser.State;
            }

            if (HttpContext.Session.GetString(SD.ssPostalCode) != null)
            {
                detailCart.OrderHeader.PostalCode = HttpContext.Session.GetString(SD.ssPostalCode);
                string[] precoPrazo = await SD.GetPriceAndTimePostalServiceAsync(detailCart.OrderHeader.PostalCode, weigth, volume);
                detailCart.OrderHeader.PostalPrice = Convert.ToDouble(precoPrazo[0]);
                detailCart.OrderHeader.PostalTime = Convert.ToDouble(precoPrazo[1]);
                detailCart.OrderHeader.OrderTotal += detailCart.OrderHeader.PostalPrice;
                HttpContext.Session.SetInt32("ssPostalPrice", Convert.ToInt32(detailCart.OrderHeader.PostalPrice * 100));
            }
            else
            {
                detailCart.OrderHeader.PostalCode = applicationUser.PostalCode;
                string[] precoPrazo = await SD.GetPriceAndTimePostalServiceAsync(detailCart.OrderHeader.PostalCode, weigth, volume);
                detailCart.OrderHeader.PostalPrice = Convert.ToDouble(precoPrazo[0]);
                detailCart.OrderHeader.PostalTime = Convert.ToDouble(precoPrazo[1]);
                detailCart.OrderHeader.OrderTotal += detailCart.OrderHeader.PostalPrice;
                HttpContext.Session.SetInt32("ssPostalPrice", Convert.ToInt32(detailCart.OrderHeader.PostalPrice * 100));
            }

            return View(detailCart);
        }

        //SUMMARY - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            detailCart.listCart = await _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToListAsync();

            detailCart.OrderHeader.UserId = claim.Value;
            detailCart.OrderHeader.Status = SD.PaymentStatusPending;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;

            var postalPrice = HttpContext.Session.GetInt32("ssPostalPrice");
            detailCart.OrderHeader.PostalPrice = Convert.ToDouble(postalPrice) / 100;

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();

            _db.OrderHeader.Add(detailCart.OrderHeader);
            await _db.SaveChangesAsync();

            detailCart.OrderHeader.OrderTotalOriginal = 0.0;

            if (claim != null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }



            foreach (var item in detailCart.listCart)
            {
                item.StoreItem = await _db.StoreItem.FirstOrDefaultAsync(m => m.Id == item.StoreItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    StoreItemId = item.StoreItemId,
                    OrderId = detailCart.OrderHeader.Id,
                    Description = item.StoreItem.Description,
                    Name = item.StoreItem.Name,
                    Price = item.StoreItem.Price,
                    Count = item.Count
                };
                detailCart.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);
            }



            detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotalOriginal + detailCart.OrderHeader.PostalPrice;

            _db.ShoppingCart.RemoveRange(detailCart.listCart);
            HttpContext.Session.SetInt32("ssCartCount", 0);
            await _db.SaveChangesAsync();

            var options = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(detailCart.OrderHeader.OrderTotal * 100),
                Currency = "brl",
                Description = "Order ID : " + detailCart.OrderHeader.Id,
                SourceId = stripeToken
            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            if (charge.BalanceTransactionId == null)
            {
                detailCart.OrderHeader.Status = SD.PaymentStatusRejected;
            }
            else
            {
                detailCart.OrderHeader.TransactionId = charge.BalanceTransactionId;
            }

            if (charge.Status.ToLower() == "succeeded")
            {
                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                detailCart.OrderHeader.Status = SD.StatusSubmitted;
            }
            else
            {
                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }

            await _db.SaveChangesAsync();
            //return RedirectToAction("Index", "Home");
            return RedirectToAction("Confirm", "Order", new { id = detailCart.OrderHeader.Id });
        }


        public IActionResult AddPostalCode()
        {
            if (detailCart.OrderHeader.PostalCode != null)
            {
                HttpContext.Session.SetString(SD.ssPostalCode, detailCart.OrderHeader.PostalCode);
            }
            if (detailCart.OrderHeader.State != null)
            {
                HttpContext.Session.SetString(SD.ssUserState, detailCart.OrderHeader.State);
            }
            if (detailCart.OrderHeader.Name != null)
            {
                HttpContext.Session.SetString(SD.ssUserName, detailCart.OrderHeader.Name);
            }
            if (detailCart.OrderHeader.City != null)
            {
                HttpContext.Session.SetString(SD.ssUserCity, detailCart.OrderHeader.City);
            }
            if (detailCart.OrderHeader.Address != null)
            {
                HttpContext.Session.SetString(SD.ssUserAddress, detailCart.OrderHeader.Address);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                await _db.SaveChangesAsync();

                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("ssCartCount", cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = await _db.ShoppingCart.FirstOrDefaultAsync(c => c.Id == cartId);
            _db.ShoppingCart.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32("ssCartCount", cnt);
            return RedirectToAction(nameof(Index));
        }

        

    }
}