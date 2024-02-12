﻿using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
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
		public async Task<IActionResult> Create()
		{
			ProductCreateDTO productCreateDTO = new ProductCreateDTO();
			productCreateDTO = await _service.CreatedAsync(productCreateDTO);
			return View(productCreateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Create(ProductCreateDTO productDTO)
		{
			if (await _service.Create(productDTO, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}
			return View(await _service.CreatedAsync(productDTO));
		}
		public async Task<IActionResult> Update(int id)
		{
			ProductUpdateDTO updateDTO = new ProductUpdateDTO();
			updateDTO = await _service.GetProductForUpdateAsync(id, updateDTO);

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, ProductUpdateDTO updateDTO)
		{
			if(await _service.Update(id, updateDTO,ModelState))

			return RedirectToAction(nameof(Index));
			return View(await _service.GetProductForUpdateAsync(id,updateDTO));
		}
		public async Task<IActionResult> Delete(int id)
		{
			if(await _service.Delete(id))
				return RedirectToAction(nameof(Index));
			return NotFound();
		}
		////public async Task<IActionResult> Details(int id)
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
