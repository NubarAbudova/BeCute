using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Categories;
using FluentValidation;

namespace EnchantElegance.Application.Validators
{
	internal class CategoryCreateDTOValidator:AbstractValidator<CategoryCreateDTO>
	{
        public CategoryCreateDTOValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).Matches(@"[a-zA-Z0-9\s]*$");
        }
    }
}
