using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Users;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Enums;
using EnchantElegance.Domain.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IWebHostEnvironment _env;

		public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> SignInManager, IMapper mapper,
			RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_signInManager = SignInManager;
			_mapper = mapper;
			_roleManager = roleManager;
			_env = env;
		}

		public async Task<List<string>> Register(RegisterDTO registerDTO)
		{
			List<string> String = new List<string>();

			if (!registerDTO.Name.IsLetter())
			{
				String.Add("Your Name or Surname only contain letters");
				return String;
			}
			if (!registerDTO.Email.CheeckEmail())
			{
				String.Add("Your Email type is not true");
				return String;
			}
			registerDTO.Name.Capitalize();
			registerDTO.Surname.Capitalize();
			AppUser user = new AppUser
			{
				Name = registerDTO.Name,
				UserName = registerDTO.UserName,
				Surname = registerDTO.Surname,
				Email = registerDTO.Email,

			};

			//if (registerDTO.MainImage is not null)
			//{
			//	if (!registerDTO.MainImage.CheckType("image/"))
			//	{
			//		String.Add("Your photo type is not true.Please use only image");
			//		return String;
			//	}
			//	if (!registerDTO.MainImage.ValidateSize(7))
			//	{
			//		String.Add("Your Email size must be max 7 mb");
			//		return String;
			//	}
			//	user.MainImage = await register.MainImage.CreateFileAsync(_env.WebRootPath, "assets", "images");
			//}
			//if (register.BackImage is not null)
			//{
			//	if (!register.BackImage.CheckType("image/"))
			//	{
			//		String.Add("Your photo type is not true.Please use only image");
			//		return String;
			//	}
			//	if (!register.BackImage.ValidateSize(7))
			//	{
			//		String.Add("Your Email size must be max 5mb");
			//		return String;
			//	}
			//	user.BackImage = await register.BackImage.CreateFileAsync(_env.WebRootPath, "assets", "images");
			//}
			IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{

					String.Add(error.Description);
				}
				return String;
			}

			await _signInManager.SignInAsync(user, isPersistent: false);
			if (user != null)
			{
				//await AssignRoleToUser(user, registerDTO.SelectedRole);
			}
			return String;

		}


		public async Task<List<string>> Login(LoginDTO loginDTO)
		{
			List<string> String = new List<string>();

			AppUser user = await _userManager.FindByEmailAsync(loginDTO.UsernameorEmail);
			if (user == null)
			{
				user = await _userManager.FindByNameAsync(loginDTO.UsernameorEmail);
				if (user == null)
				{
					String.Add("Username, Email or Password was wrong");
					return String;

				}
			}
			var result = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.IsRemembered, true);
			if (result.IsLockedOut)
			{
				String.Add("You have a lot of fail  try that is why you banned please try some minuts late");
				return String;
			}
			if (!result.Succeeded)
				if (!result.Succeeded)
				{
					String.Add("Username, Email or Password was wrong");
					return String;
				}
			return String;
		}

		public async Task CreateRoleAsync()
		{
			foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
			{
				if (!await _roleManager.RoleExistsAsync(role.ToString()))
				{
					await _roleManager.CreateAsync(new IdentityRole
					{
						Name = role.ToString(),
					});

				}
			}
		}


		public async Task CreateAdminRoleAsync()
		{
			var adminRoleName = "Admin";

			if (!await _roleManager.RoleExistsAsync(adminRoleName))
			{
				await _roleManager.CreateAsync(new IdentityRole
				{
					Name = adminRoleName,
				});
			}
		}

		public async Task AssignRoleToUser(AppUser user, string roleName)
		{
			if (!await _roleManager.RoleExistsAsync(roleName))
			{

				await _roleManager.CreateAsync(new IdentityRole
				{
					Name = roleName
				});
			}
			await _userManager.AddToRoleAsync(user, roleName);
		}

		public Task Logout()
		{
			throw new NotImplementedException();
		}
	}
}
