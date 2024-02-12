using EnchantElegance.Application.DTOs;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IAuthService
	{
		Task<List<string>> Register(DTOs.RegisterDTO registerDTO);
		Task<List<string>> Login(LoginDTO loginDTO);
		Task Logout();
	}
}
