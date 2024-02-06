using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Application.DTOs.Sliders;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class CategoryController : Controller
	{
		private readonly ICategoryService _service;

		public CategoryController(ICategoryService service)
		{
			_service = service;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _service.GetAllAsync(1, 3));
		}
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateDTO categoryCreateDTO)
		{
			if (ModelState.IsValid) return View(categoryCreateDTO);

			var result = await _service.Create(categoryCreateDTO);

			if (result.Any())
			{
				ModelState.AddModelError(String.Empty, "Create is not success");
				return View(categoryCreateDTO);
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0) return BadRequest();

			await _service.GetCategoryForUpdateAsync(id);

			CategoryUpdateDTO updateDTO = new CategoryUpdateDTO();

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, CategoryUpdateDTO updateDTO)
		{
			if (!ModelState.IsValid) return View(updateDTO);

			await _service.Update(id, updateDTO);
			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (id <= 0) return BadRequest();

			await _service.Delete(id);
			return RedirectToAction(nameof(Index));
		}
		//public async Task<IActionResult> Details(int id)
		//{
		//	if (id <= 0) BadRequest();
		//	Category Category = await _context.Categorys.FirstOrDefaultAsync(c => c.Id == id);

		//	List<Category> Categorys = await _context.Categorys
		//		.Where(s => s.Id == id)
		//		.Include(s => s.Title)
		//		.Include(s => s.SubTitle)
		//		.Include(s => s.Description)
		//		.Include(s => s.Image)
		//		.ToListAsync();

		//	if (Category == null) return NotFound();
		//	return View(Category);
		//}
	}
}
