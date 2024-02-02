using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
		private readonly AppDbContext _context;

		public CategoryController(AppDbContext context)
        {
			_context = context;
		}
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _context.Categories.ToListAsync();
            return View(categories);
        }
        public async Task<IActionResult> Create(CategoryCreateDTO categorydto)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}
			bool result = await _context.Categories.AnyAsync(c => c.Name.Trim() == categorydto.Name.Trim());
			if (result)
			{
				ModelState.AddModelError("Name", "Category name already exists");
				return View();
			}
			Category category = new Category
			{
				Name = categorydto.Name,
				Description = categorydto.Description,
			};
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
        }
    }
}
