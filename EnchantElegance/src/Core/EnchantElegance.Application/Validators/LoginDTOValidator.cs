using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using FluentValidation;

namespace EnchantElegance.Application.Validators
{
	public class LoginDTOValidator : AbstractValidator<LoginDTO>
	{
		public LoginDTOValidator()
		{
			RuleFor(x => x.UsernameorEmail)
				.NotEmpty().WithMessage("Username or Email cannot be empty.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password cannot be empty.");
		}
	}
}
