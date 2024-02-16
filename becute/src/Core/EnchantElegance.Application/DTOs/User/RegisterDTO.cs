using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Enums;

namespace EnchantElegance.Application.DTOs
{
	public class RegisterDTO
	{
		[Required(ErrorMessage = "Username is required.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format. Please enter a valid email address.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
		public string Password { get; set; }

		[Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Surname is required.")]
		public string Surname { get; set; }

		public UserRole Role { get; set; }

	}
}
