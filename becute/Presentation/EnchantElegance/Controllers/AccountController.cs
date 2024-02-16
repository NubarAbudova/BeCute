using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.DTOs.User;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _accountservice;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMailService _mailService;

		public AccountController(IAccountService service,UserManager<AppUser>userManager,IMailService mailService)
		{
			_accountservice = service;
			_userManager = userManager;
			_mailService = mailService;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			if (ModelState.IsValid)
			{
				bool registrationResult = await _accountservice.Register(registerDTO, ModelState);

				if (registrationResult)
				{
					return RedirectToAction("Index", "Home");
				}
			}
			return View(registerDTO);
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			if (ModelState.IsValid)
			{
				bool loginResult = await _accountservice.Login(loginDTO, ModelState);

				if (loginResult)
				{
					return RedirectToAction("Index", "Home");
				}
			}
			return View(loginDTO);
		}

		public async Task<IActionResult> Logout()
		{
			await _accountservice.Logout();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> CreateRoles()
		{
			await _accountservice.CreateRoleAsync();
			return RedirectToAction("Index", "Home");
		}
		public IActionResult ForgotPassword()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
		{
			if (!ModelState.IsValid) return View(forgotPasswordDTO);
			var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
			if (user != null) return NotFound();
			//https://localhost:7122/User/ResetPassword/userId?token=resetToken
			string token = await _userManager.GeneratePasswordResetTokenAsync(user);
			string link = Url.Action("ResetPassword", "User", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);

			await _mailService.SendEmailAsync(new MailRequestDTO { ToEmail = forgotPasswordDTO.Email, Subject = "ResetPassword", Body = $"<a href='{link}'>ResetPassword</a>" });


			return RedirectToAction(nameof(Login));
		}
		public async Task<IActionResult> ResetPassword(string userId, string token)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null) return NotFound();
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO, string userId, string token)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
			if (!ModelState.IsValid) return View(resetPasswordDTO);
			var user = await _userManager.FindByIdAsync(userId);
			if (user != null) return NotFound();
			var IdentityUser = await _userManager.ResetPasswordAsync(user, token, resetPasswordDTO.ConfirmPassword);
			return RedirectToAction(nameof(Login));
		}

	}

}

