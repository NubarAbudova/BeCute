using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using EnchantElegance.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			List<Category> categories = await _context.Categories.ToListAsync();
			List<Product> products = await _context.Products.Include(p=>p.ProductImages).ToListAsync();


			HomeVM homeVM = new HomeVM()
			{
				Sliders = sliders,
				Categories = categories,
				Products = products
			};

			return View(homeVM);
		}
		public IActionResult CosmeticSpa()
		{
			return View();
		}
	}
}
