using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IAuthService
	{
		Task<List<string>> Register(DTOs.RegisterDTO registerDTO);
		Task<List<string>> Login(LoginDTO loginDTO);
		Task Logout();
	
	}
}
