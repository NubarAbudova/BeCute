using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Users;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
	public class UserController : Controller
	{
		private readonly IAuthService _service;

		public UserController(IAuthService service)
		{
			_service = service;
		}

		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			await _service.Register(registerDTO);
			return RedirectToAction("Index", "Home");
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			await _service.Login(loginDTO);
			return RedirectToAction("Index", "Home");
		}
		public async Task<IActionResult> LogOut()
		{
			await _service.Logout();
			return RedirectToAction("Index", "Home");

		}

	}
}
