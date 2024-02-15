using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stripe;

namespace EnchantElegance.Controllers
{
	public class BasketController : Controller
	{
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<IActionResult> Index()
        {
            var basketItems = await _basketService.GetBasketItems();
            return View(basketItems);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int productId)
        {
            var success = await _basketService.AddToBasket(productId);
            if (success)
                return RedirectToAction("Index");

            return View("Error");
        }
        //public IActionResult GetBasket()
        //{
        //	return Content(Request.Cookies["Basket"]);
        //}
    }
}
