//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using EnchantElegance.Application.DTOs;
//using FluentValidation;

//namespace EnchantElegance.Application.Validators
//{
//	internal class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
//	{
//		public ProductCreateDTOValidator()
//		{
//			RuleFor(x => x.Name)
//				.NotEmpty().WithMessage("Name must be entered")
//				.MaximumLength(100);

//			RuleFor(x => x.Description)
//			   .NotEmpty().WithMessage("Description must be entered")
//			   .MaximumLength(255).WithMessage("Description cannot exceed 255 characters");

//			RuleFor(x => x.OldPrice)
//				.GreaterThan(0).WithMessage("Price must be greater than 0");

//			RuleFor(x => x.CurrentPrice)
//				.GreaterThan(0).WithMessage("Price must be greater than 0");

//			RuleFor(x => x.CategoryId)
//				.GreaterThan(0).WithMessage("Category ID must be greater than 0");

//			RuleFor(x => x.ColorId)
//				.GreaterThan(0).WithMessage("Category ID must be greater than 0");
//		}
//		public bool CheckPrice(decimal price)
//		{
//			if (price >= 10 && price <= 999999.99m) return true;
//			return false;
//		}
//	}
//}


