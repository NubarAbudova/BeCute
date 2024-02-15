using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Enums;
using EnchantElegance.Domain.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
		
		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}
		
	}
}
