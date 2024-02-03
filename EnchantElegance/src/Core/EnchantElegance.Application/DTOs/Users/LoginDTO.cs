using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs.Users
{
	public record LoginDTO(string UsernameorEmail,string Password,bool IsRemembered);

}
