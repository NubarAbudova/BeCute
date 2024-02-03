using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Users;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnchantElegance.Controllers
{
	public class UserController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDTO registerDTO)
		{
			if (!ModelState.IsValid) return View(registerDTO);

			AppUser user = new AppUser()
			{
				Name = registerDTO.Name,
				Surname = registerDTO.Surname,
				UserName = registerDTO.UserName,
				Email = registerDTO.Email,
			};

			IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
			if (!result.Succeeded)
			{
				foreach (IdentityError error in result.Errors)
				{
					ModelState.AddModelError(String.Empty, error.Description);
					return View();
				}
			}

			//await _userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
			await _signInManager.SignInAsync(user, false);
			return RedirectToAction("Index", "Home", new { Area = "" });

		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDTO loginDTO)
		{
			if (!ModelState.IsValid) return View(loginDTO);

			AppUser user = await _userManager.FindByNameAsync(loginDTO.UsernameorEmail);
			if (user == null)
			{
				user = await _userManager.FindByEmailAsync(loginDTO.UsernameorEmail);
				if (user == null)
				{
					ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect");
					return View();
				}
			}

			var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.IsRemembered, true);
			if (result.IsLockedOut)
			{
				ModelState.AddModelError(String.Empty, "Login has been blocked to wrong attemps,please try again later");
			}
			if (!result.Succeeded)
			{
				ModelState.AddModelError(String.Empty, "Username,Email or password is incorrect");
				return View();
			}

			return RedirectToAction("Index", "Home", new { Area = "" });
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home", new { Area = "" });
		}

		//public async Task<IActionResult> CreateRoles()
		//{
		//	foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
		//	{
		//		if (!await _roleManager.RoleExistsAsync(role.ToString()))
		//		{
		//			await _roleManager.CreateAsync(new IdentityRole()
		//			{
		//				Name = role.ToString()
		//			});
		//		}
		//	}
		//	return RedirectToAction("Index", "Home", new { Area = "" });

		//}


	}
}
