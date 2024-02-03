using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public AuthService(UserManager<AppUser> userManager)
        {
			_userManager = userManager;
		}
        public async Task Register(RegisterDTO registerDTO)
		{
			if (await _userManager.Users.AnyAsync(u => u.UserName == registerDTO.UserName || u.Email == registerDTO.Email))
				throw new Exception("Username or email is already available");
		}
	}
}
