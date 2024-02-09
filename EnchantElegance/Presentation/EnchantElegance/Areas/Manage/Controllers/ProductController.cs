using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class ProductController : Controller
	{
		private readonly IProductService _service;

		public ProductController(IProductService service)
		{
			_service = service;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _service.GetAllAsync(1, 3));
		}
		public IActionResult Create()
		{
			var productCreateDTO = _service.GetProductCreateDTO();
			// Diğer işlemler...
			return View(productCreateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateDTO productDTO)
		{
			if (!ModelState.IsValid) return View(productDTO);


			var result = await _service.Create(productDTO);

			if (result.Any())
			{
				ModelState.AddModelError(String.Empty, "Create is not success");
				return View(productDTO);
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			if (id <= 0) return BadRequest();

			ProductUpdateDTO updateDTO = await _service.GetProductForUpdateAsync(id);

			if (updateDTO == null) return NotFound();
	
			return View(updateDTO);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int id, ProductUpdateDTO updateDTO)
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
