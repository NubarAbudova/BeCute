using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IAccountService
	{
		Task<bool> Register(RegisterDTO dto, ModelStateDictionary modelstate);
		Task<bool> Login(LoginDTO dto, ModelStateDictionary modelstate);
		Task Logout();
		Task CreateRoleAsync();
        Task<AppUser> GetUserAsync(string userName);
        Task AssignRoleToUser(AppUser user, string roleName);
	}
}
