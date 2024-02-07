using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Users;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IAuthService
	{
		Task<List<string>> Register(RegisterDTO registerDTO);
		Task<List<string>> Login(LoginDTO loginDTO);
		Task Logout();
		Task CreateRoleAsync();
		Task CreateAdminRoleAsync();
		Task AssignRoleToUser(AppUser user, string roleName);
	}
}
