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
        public async Task<IActionResult> Create(CategoryCreateDTO createDTO)
        {
			if (!ModelState.IsValid) return View();
			bool result = await _context.Categories.AnyAsync(c => c.Name.Trim() == createDTO.Name.Trim());
			if (result)
			{
				ModelState.AddModelError("Name", "Category name already exists");
				return View();
			}
			Category category = new Category
			{
				Name = createDTO.Name,
				Description = createDTO.Description,
			};
			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
        }
		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0) return BadRequest();
			Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
			if (category == null) return NotFound();
			CategoryUpdateDTO updateDTO = new CategoryUpdateDTO(category.Id, category.Name, category.Description);
			return View(updateDTO);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int id, CategoryUpdateDTO updateDTO)
		{
			if (!ModelState.IsValid) return View();

			Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

			if (existed == null) return NotFound();

			if (existed.Name != updateDTO.Name && !string.IsNullOrEmpty(updateDTO.Name))
			{
				bool result = await _context.Categories.AnyAsync(c => c.Name == updateDTO.Name);

				if (result)
				{
					ModelState.AddModelError("Name", "Category name already exists");
					return View();
				}
			}

			existed.Name = updateDTO.Name;
			existed.Description = updateDTO.Description;

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

	}
}
