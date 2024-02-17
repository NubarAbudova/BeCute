﻿using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Application.Abstarctions.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EnchantElegance.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class SliderController : Controller
	{
		private readonly ISliderService _service;

		public SliderController(ISliderService service)
		{
			_service = service;
		}
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Index(int page = 1, int take = 3)
		{

			PaginationVM<Slider> vm;
			vm = await _service.GetAllAsync(page, take);
			if (vm.Items == null)
				return NotFound();
			return View(vm);
		}
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(SliderCreateDTO sliderDTO)
		{
			if (await _service.Create(sliderDTO, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}
			return View(sliderDTO);
		}
		[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Update(int id)
		{
			SliderUpdateDTO updateDTO = new SliderUpdateDTO();
			updateDTO = await _service.GetProductForUpdateAsync(id, updateDTO);

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, SliderUpdateDTO updateDTO)
		{
			if (await _service.Update(id, updateDTO, ModelState))
				return RedirectToAction(nameof(Index));
			return View(await _service.GetProductForUpdateAsync(id,updateDTO));
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
		//	Slider slider = await _context.Sliders.FirstOrDefaultAsync(c => c.Id == id);

		//	List<Slider> sliders = await _context.Sliders
		//		.Where(s => s.Id == id)
		//		.Include(s => s.Title)
		//		.Include(s => s.SubTitle)
		//		.Include(s => s.Description)
		//		.Include(s => s.Image)
		//		.ToListAsync();

		//	if (slider == null) return NotFound();
		//	return View(slider);
		//}
	}
}