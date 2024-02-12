using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class ColorController : Controller
	{
		private readonly IColorService _service;

		public ColorController(IColorService service)
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
		public async Task<IActionResult> Create(ColorCreateDTO colorDTO)
		{
			if (await _service.Create(colorDTO, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}
			return View(colorDTO);
		}
		public async Task<IActionResult> Update(int id)
		{
			ColorUpdateDTO updateDTO = new ColorUpdateDTO();
			updateDTO = await _service.GetColorForUpdateAsync(id, updateDTO);

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, ColorUpdateDTO updateDTO)
		{
			if (await _service.Update(id, updateDTO, ModelState))
				return RedirectToAction(nameof(Index));
			return View(await _service.Update(id, updateDTO, ModelState));
		}
		public async Task<IActionResult> Delete(int id)
		{
			if (await _service.Delete(id))
				return RedirectToAction(nameof(Index));
			return NotFound();
		}
	}
}
