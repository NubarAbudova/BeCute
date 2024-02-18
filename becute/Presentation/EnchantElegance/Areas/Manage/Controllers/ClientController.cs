using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ClientController : Controller
    {
        private readonly IClientService _service;

        public ClientController(IClientService service)
        {
            _service = service;
        }
        //[Authorize(Roles = "SuperAdministrator,Administrator")]

        public async Task<IActionResult> Index(int page = 1, int take = 3)
        {

            PaginationVM<Client> vm;
            vm = await _service.GetAllAsync(page, take);
            if (vm.Items == null)
                return NotFound();
            return View(vm);
        }
        //[Authorize(Roles = "SuperAdministrator,Administrator")]

        public async Task<IActionResult> Create()
        {
            ClientCreateDTO ClientCreateDTO = new ClientCreateDTO();
            ClientCreateDTO = await _service.CreatedAsync(ClientCreateDTO);
            return View(ClientCreateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateDTO ClientDTO)
        {
            if (await _service.Create(ClientDTO, ModelState))
            {
                return RedirectToAction(nameof(Index));
            }
            return View(ClientDTO);
        }
        //[Authorize(Roles = "SuperAdministrator,Administrator")]

        public async Task<IActionResult> Update(int id)
        {
            ClientUpdateDTO updateDTO = new ClientUpdateDTO();
            updateDTO = await _service.GetClientForUpdateAsync(id, updateDTO);

            return View(updateDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ClientUpdateDTO updateDTO)
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