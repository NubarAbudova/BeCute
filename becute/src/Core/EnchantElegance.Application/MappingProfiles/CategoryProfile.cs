using AutoMapper;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.MappingProfiles
{
	internal class CategoryProfile : Profile
	{
		public CategoryProfile()
		{
			CreateMap<Category, CategoryItemDTO>().ReverseMap();

			CreateMap<CategoryCreateDTO, Category>();

			CreateMap<CategoryUpdateDTO, Category>().ReverseMap();

		}
	}
}
