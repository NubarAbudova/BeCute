using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Users;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IMapper _mapper;

		public AuthService(UserManager<AppUser> userManager,SignInManager<AppUser>SignInManager, IMapper mapper)
        {
			_userManager = userManager;
			_signInManager = SignInManager;
			_mapper = mapper;
		}

		public async Task Register(RegisterDTO registerDTO)
		{
			if (await _userManager.Users.AnyAsync(u => u.UserName == registerDTO.UserName || u.Email == registerDTO.Email))
				throw new Exception("Username or email is already available");

			AppUser user=_mapper.Map<AppUser>(registerDTO);

			var result=await _userManager.CreateAsync(user,registerDTO.Password);

			if (!result.Succeeded)
			{
				StringBuilder builder= new StringBuilder();
				foreach (var error in result.Errors)
				{
					builder.AppendLine(error.Description);
				}
				throw new Exception(builder.ToString());
			}

		}
		public async Task Login(LoginDTO loginDTO)
		{
			AppUser user = await _userManager.FindByNameAsync(loginDTO.UsernameorEmail);
			if (user is null)
			{
				user = await _userManager.FindByEmailAsync(loginDTO.UsernameorEmail);
				if (user is null) throw new Exception("Username,email or password is incorrect");
			}
			if (!await _userManager.CheckPasswordAsync(user, loginDTO.Password)) throw new Exception("Username,email or password is incorrect");
		}
		public async Task Logout()
		{
			await _signInManager.SignOutAsync();
		
		}

	}
}
