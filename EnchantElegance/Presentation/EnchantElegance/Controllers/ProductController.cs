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
		private readonly AppDbContext _context;

		public ProductController(AppDbContext context)
        {
			_context = context;
		}

		//public IActionResult Index()
		//{
		//	return View();
		//}

		public async Task<IActionResult> Detail(int id)
		{
			if (id <= 0) return BadRequest();

			Product product = _context.Products
				.Include(p => p.Category)
				.Include(p=>p.ProductImages)
				.Include(p=>p.ProductColors).ThenInclude(pc=>pc.Color)
				.FirstOrDefault(p => p.Id == id);

			if (product == null) return NotFound();

            List<Product> relatedproducts = await _context.Products
               .Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id).Take(8)
               .Include(p => p.Category)
               .Include(p => p.ProductImages.Where(pi => pi.IsPrimary == null))
            .ToListAsync();


            DetailVM vm = new DetailVM
            {
                Product = product,
                RelatedProducts = relatedproducts
            };
            return View(vm);
		}
	}
}
