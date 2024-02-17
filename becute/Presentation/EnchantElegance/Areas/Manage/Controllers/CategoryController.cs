﻿using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Index(int page = 1, int take = 3)
		{

            PaginationVM<Category> vm;
            vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null)
                return NotFound();
            return View(vm);
        }
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Create()
		{
			CategoryCreateDTO categoryCreateDTO = new CategoryCreateDTO();
			categoryCreateDTO = await _service.CreatedAsync(categoryCreateDTO);
			return View(categoryCreateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Create(CategoryCreateDTO categoryDTO)
		{
			if (await _service.Create(categoryDTO, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}
			return View(categoryDTO);
		}
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Update(int id)
		{
			CategoryUpdateDTO updateDTO = new CategoryUpdateDTO();
			updateDTO = await _service.GetCategoryForUpdateAsync(id, updateDTO);

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, CategoryUpdateDTO updateDTO)
		{
			if (await _service.Update(id, updateDTO, ModelState))
				return RedirectToAction(nameof(Index));
			return View(await _service.Update(id, updateDTO, ModelState));
		}
		[Authorize(Roles = "SuperAdministrator")]

		public async Task<IActionResult> Delete(int id)
		{
			if (await _service.Delete(id))
				return RedirectToAction(nameof(Index));
			return NotFound();
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