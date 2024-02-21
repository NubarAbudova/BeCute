using EnchantElegance.Application.Abstarctions.Services;
using Microsoft.AspNetCore.Mvc;
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
		
		public async Task<IActionResult> AddToBasket(int productId)
		{
			var success = await _basketService.AddToBasket(productId);
			if (success)
				return RedirectToAction("Index","Home");
			return View(nameof(Index));
		}
        public async Task<IActionResult> Remove(int id)
        {
            await _basketService.Remove(id);
            return RedirectToAction("Index", "Basket");
        }
        public async Task<IActionResult> Minus(int id)
        {
            await _basketService.Minus(id);
            return RedirectToAction("Index", "Basket");
        }
        public async Task<IActionResult> Plus(int id)
        {
            await _basketService.Plus(id);
            return RedirectToAction("Index", "Basket");
        }
    }
}
