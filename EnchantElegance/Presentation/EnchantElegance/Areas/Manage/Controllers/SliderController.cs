using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Application.Abstarctions.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
		public async Task<IActionResult> Index()
		{
			return View(await _service.GetAllAsync(1, 3));
		}
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Create(SliderCreateDTO sliderDTO)
		{
			if (ModelState.IsValid) return View(sliderDTO);

			var result = await _service.Create(sliderDTO);

			if (result.Any())
			{
				ModelState.AddModelError(String.Empty, "Create is not success");
				return View(sliderDTO);
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
		
			if (id <= 0) return BadRequest();

			SliderUpdateDTO updateDTO = await _service.GetSliderForUpdateAsync(id);
			if (updateDTO == null) return NotFound();
			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, SliderUpdateDTO updateDTO)
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
