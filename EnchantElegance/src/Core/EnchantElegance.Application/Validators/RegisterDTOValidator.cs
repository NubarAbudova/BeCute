using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using FluentValidation;

namespace EnchantElegance.Application.Validators
{
	public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
	{
		public RegisterDTOValidator()
		{
			RuleFor(x => x.UserName)
			  .NotEmpty().WithMessage("Username cannot be empty.")
			  .MaximumLength(100).WithMessage("Username must be at most 100 characters long.");

			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email cannot be empty.")
				.MaximumLength(256).WithMessage("Email must be at most 256 characters long.")
				.Must(BeAValidEmail).WithMessage("Please enter a valid email address.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password cannot be empty.")
				.Must(BeAValidPassword).WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, and one digit.");

			RuleFor(x => x.ConfirmPassword)
				.NotEmpty().WithMessage("Password confirmation cannot be empty.")
				.Equal(x => x.Password).WithMessage("Passwords do not match.");

			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name cannot be empty.")
			  .MaximumLength(50).WithMessage("Name must be at most 100 characters long.");


			RuleFor(x => x.Surname)
				.NotEmpty().WithMessage("Surname cannot be empty.")
			  .MaximumLength(50).WithMessage("Surname must be at most 100 characters long.");


			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Phone number cannot be empty.")
				.Must(BeAValidPhoneNumber).WithMessage("Please enter a valid phone number.");
		}

		private bool BeAValidPassword(string password)
		{
			return !string.IsNullOrEmpty(password) &&
				   password.Length >= 8 &&
				   password.Any(char.IsUpper) &&
				   password.Any(char.IsLower) &&
				   password.Any(char.IsDigit);
		}

		private bool BeAValidEmail(string email)
		{
			return !string.IsNullOrEmpty(email) && new System.Net.Mail.MailAddress(email).Address == email;
		}
		private bool BeAValidPhoneNumber(string phoneNumber)
		{
			return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 10;
		}
	}

}


