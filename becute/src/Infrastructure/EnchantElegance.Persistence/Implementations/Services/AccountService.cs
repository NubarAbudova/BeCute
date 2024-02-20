using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> SignInManager, IMapper mapper,
			RoleManager<IdentityRole> roleManager, IWebHostEnvironment env)
		{
			_userManager = userManager;
			_signInManager = SignInManager;
			_roleManager = roleManager;
		}


		public async Task<bool> Register(RegisterDTO dto, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;

			AppUser user = new AppUser
			{
				Name = dto.Name,
				UserName = dto.UserName,
				Surname = dto.Surname,
				Email = dto.Email,

			};

			var result = await _userManager.CreateAsync(user, dto.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{

					modelstate.AddModelError(string.Empty, error.Description);

				}
				return false;
			}

			await _userManager.AddToRoleAsync(user,UserRole.SuperAdministrator.ToString());

			await _signInManager.SignInAsync(user, isPersistent: false);
			if (user != null)
			{
				await AssignRoleToUser(user, dto.Role.ToString());
			}
			return true;

		}

		public async Task<bool> Login(LoginDTO dto, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;

			AppUser user = await _userManager.FindByEmailAsync(dto.UsernameorEmail);
			if (user is null)
			{
				user = await _userManager.FindByNameAsync(dto.UsernameorEmail);
				if (user is null)
				{
					modelstate.AddModelError(string.Empty, "Account not found. Please enter a valid username,email or password.");
					return false;
				}
			}
			var result = await _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsRemembered, true);
			if (result.IsLockedOut)
			{
				modelstate.AddModelError(string.Empty, "Your account is locked. Please try again after a few minutes.");
				return false;
			}
			if (!result.Succeeded)
			{
				modelstate.AddModelError(string.Empty, "Incorrect password,username or email. Please make sure you entered the correct information.");
				return false;
			}

			return true;

		}
		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		}
        public async Task<AppUser> GetUserAsync(string userName)
        {
            return await _userManager.Users.Include(x => x.Products).Include(x => x.BasketItems).Include(x => x.Orders).FirstOrDefaultAsync(x => x.UserName == userName);
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
		public async Task AssignRoleToUser(AppUser user, string role)
		{

			if (!await _roleManager.RoleExistsAsync(role))
			{

				await _roleManager.CreateAsync(new IdentityRole
				{
					Name = role
				});
			}
			await _userManager.AddToRoleAsync(user, role);
		}


	}
}
