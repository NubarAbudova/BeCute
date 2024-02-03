using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
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
            return View(sliders);
        }
		public IActionResult CosmeticSpa()
		{
			return View();
		}
	}
}
