using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs
{
	public class LoginDTO
	{
		[Required(ErrorMessage = "Username or email is required.")]
		public string UsernameorEmail { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
		[DataType(DataType.Password, ErrorMessage = "Invalid password format.")]
		public string Password { get; set; }

		public bool IsRemembered { get; set; }

	}

}
