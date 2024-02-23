using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace EnchantElegance.Controllers
{
    public class ShopController : Controller
    {
		private readonly IShopService _shopService;

		public ShopController(IShopService shopService)
        {
			_shopService = shopService;
		}
        public async Task<IActionResult> Index(string? search, int? order, int? categoryId, int page = 1, int take = 3)
        {
            var orderitems = await _shopService.GetShopItems(search,order,categoryId,page,take);
                return View(orderitems);
        }
    }
}