using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FishStore.Data;
using FishStore.Models.ViewModels;
using FishStore.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> Index()
        {
            detailCart = new OrderDetailsCart()
            {
                OrderHeader = new Models.OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0.0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);
            if(cart != null)
            {
                detailCart.listCart = cart.ToList();
            }

            double weigth = 0.0;
            double volume = 0.0;

            foreach(var list in detailCart.listCart)
            {
                list.StoreItem = await _db.StoreItem.FirstOrDefaultAsync(m => m.Id == list.StoreItemId);
                detailCart.OrderHeader.OrderTotal = detailCart.OrderHeader.OrderTotal + (list.StoreItem.Price * list.Count);
                list.StoreItem.Description = SD.ConvertToRawHtml(list.StoreItem.Description);
                weigth += list.StoreItem.Weight;
                volume += list.StoreItem.Volume;
            }
            detailCart.OrderHeader.OrderTotalOriginal = detailCart.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssPostalCode) != null)
            {
                detailCart.OrderHeader.PostalCode = HttpContext.Session.GetString(SD.ssPostalCode);
                string[] precoPrazo = await SD.GetPriceAndTimePostalServiceAsync(detailCart.OrderHeader.PostalCode, weigth, volume);
                detailCart.OrderHeader.PostalPrice = Convert.ToDouble(precoPrazo[0]);
                detailCart.OrderHeader.PostalTime = Convert.ToDouble(precoPrazo[1]);
                detailCart.OrderHeader.OrderTotal += detailCart.OrderHeader.PostalPrice;
            }

            return View(detailCart);
        }

        public IActionResult AddPostalCode()
        {
            if (detailCart.OrderHeader.PostalCode == null)
            {
                detailCart.OrderHeader.PostalCode = "";
            }
            HttpContext.Session.SetString(SD.ssPostalCode, detailCart.OrderHeader.PostalCode);

            return RedirectToAction(nameof(Index));
        }
    }
}