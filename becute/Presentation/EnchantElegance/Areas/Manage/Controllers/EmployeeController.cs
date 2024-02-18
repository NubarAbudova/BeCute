using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Areas.Manage.Controllers
{
	[Area("Manage")]
	public class EmployeeController : Controller
	{
		private readonly IEmployeeService _service;

		public EmployeeController(IEmployeeService service)
		{
			_service = service;
		}
		//[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Index(int page = 1, int take = 3)
		{

			PaginationVM<Employee> vm;
			vm = await _service.GetAllAsync(page, take);
			if (vm.Items == null)
				return NotFound();
			return View(vm);
		}
		//[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Create()
		{
			EmployeeCreateDTO EmployeeCreateDTO = new EmployeeCreateDTO();
			EmployeeCreateDTO = await _service.CreatedAsync(EmployeeCreateDTO);
			return View(EmployeeCreateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Create(EmployeeCreateDTO EmployeeDTO)
		{
			if (await _service.Create(EmployeeDTO, ModelState))
			{
				return RedirectToAction(nameof(Index));
			}
			return View(EmployeeDTO);
		}
		//[Authorize(Roles = "SuperAdministrator,Administrator")]

		public async Task<IActionResult> Update(int id)
		{
			EmployeeUpdateDTO updateDTO = new EmployeeUpdateDTO();
			updateDTO = await _service.GetEmployeeForUpdateAsync(id, updateDTO);

			return View(updateDTO);
		}
		[HttpPost]
		public async Task<IActionResult> Update(int id, EmployeeUpdateDTO updateDTO)
		{
			if (await _service.Update(id, updateDTO, ModelState))
				return RedirectToAction(nameof(Index));
			return View(await _service.Update(id, updateDTO, ModelState));
		}
		//[Authorize(Roles = "SuperAdministrator")]

		public async Task<IActionResult> Delete(int id)
		{
			if (await _service.Delete(id))
				return RedirectToAction(nameof(Index));
			return NotFound();
		}
	}
}
