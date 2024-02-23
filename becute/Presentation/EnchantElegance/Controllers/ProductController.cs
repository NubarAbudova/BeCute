using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.ViewModels;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Controllers
{
	public class ProductController : Controller
	{
        private readonly IProductRepository _productrepo;
        private readonly IBasketItemRepository _basketitemrepo;

        public ProductController(IProductRepository productrepo,IBasketItemRepository basketitemrepo)
        {
            _productrepo = productrepo;
            _basketitemrepo = basketitemrepo;
        }

		//public IActionResult Index()
		//{
		//	return View();
		//}

		public async Task<IActionResult> Detail(int id)
		{
			if (id <= 0) return BadRequest();

			Product product = _productrepo.GetAll()
				.Include(p => p.Category)
				.Include(p=>p.ProductImages)
				.Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
				.FirstOrDefault(p => p.Id == id);

			if (product == null) return NotFound();

            List<Product> trendingproducts = await _productrepo.GetAll()
               .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).Take(8)
               .Include(p => p.Category)
               .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == null))
            .ToListAsync();

			List<BasketItem> items = await _basketitemrepo.GetAll().ToListAsync();

			DetailVM vm = new DetailVM
            {
                Product = product,
                TrendingProducts = trendingproducts,
				Items=items
            };
            return View(vm);
		}
	}
}
