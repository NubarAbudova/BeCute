using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Users;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IAuthService
	{
		Task Register(RegisterDTO registerDTO);
	}
}
