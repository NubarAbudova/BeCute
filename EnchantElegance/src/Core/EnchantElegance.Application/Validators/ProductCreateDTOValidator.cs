using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Products;
using FluentValidation;

namespace EnchantElegance.Application.Validators
{
	internal class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
	{
		public ProductCreateDTOValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty().WithMessage("Name must be entered")
				.MaximumLength(100);

			RuleFor(x => x.Description)
			   .NotEmpty().WithMessage("Description must be entered")
			   .MaximumLength(255).WithMessage("Description cannot exceed 255 characters");

			RuleFor(x => x.Price)
				.GreaterThan(0).WithMessage("Price must be greater than 0");

			RuleFor(x => x.StockQuantity)
				.GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");

			RuleFor(x => x.Brand)
				.NotEmpty().WithMessage("Brand must be entered")
				.MaximumLength(50).WithMessage("Brand cannot exceed 50 characters");

			RuleFor(x => x.SkinType)
				.NotEmpty().WithMessage("Skin type must be entered")
				.MaximumLength(50).WithMessage("Skin type cannot exceed 50 characters");

			RuleFor(x => x.SkinTone)
				.NotEmpty().WithMessage("Skin tone must be entered")
				.MaximumLength(50).WithMessage("Skin tone cannot exceed 50 characters");

			RuleFor(x => x.Color)
				.MaximumLength(50).WithMessage("Color cannot exceed 50 characters");

			RuleFor(x => x.Usage)
				.NotEmpty().WithMessage("Usage must be entered")
				.MaximumLength(100).WithMessage("Usage cannot exceed 100 characters");

			RuleFor(x => x.Ingredients)
				.NotEmpty().WithMessage("Ingredients must be entered");

			RuleFor(x => x.IsOrganic)
				.NotNull().WithMessage("IsOrganic must be specified");

			RuleFor(x => x.CategoryId)
				.GreaterThan(0).WithMessage("Category ID must be greater than 0");
		}
		public bool CheckPrice(decimal price)
		{
			if (price >= 10 && price <= 999999.99m) return true;
			return false;
		}
	}
}


