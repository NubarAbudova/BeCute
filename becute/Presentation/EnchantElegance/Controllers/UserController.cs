using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.DTOs.User;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Implementations.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
	public class UserController : Controller
	{
		private readonly IAuthService _service;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMailService _mailService;

		public UserController(IAuthService service,UserManager<AppUser>userManager,IMailService mailService)
		{
			_service = service;
			_userManager = userManager;
			_mailService = mailService;
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
		public IActionResult ForgotPassword()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
		{
			if(!ModelState.IsValid) return View(forgotPasswordDTO);
			var user=await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
			if (user != null) return NotFound();
			//https://localhost:7122/User/ResetPassword/userId?token=resetToken
			string token=await _userManager.GeneratePasswordResetTokenAsync(user);
			string link = Url.Action("ResetPassword", "User", new {userId=user.Id,token=token},HttpContext.Request.Scheme);

			await _mailService.SendEmailAsync(new MailRequestDTO { ToEmail = forgotPasswordDTO.Email, Subject = "ResetPassword", Body = $"<a href='{link}'>ResetPassword</a>" });
			
			
			return RedirectToAction(nameof(Login));
		}
		public async Task<IActionResult>ResetPassword(string userId,string token)
		{
			if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
			var user=await _userManager.FindByIdAsync(userId);
			if(user != null) return NotFound();
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
