using AutoMapper;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.MappingProfiles
{
	internal class ColorProfile:Profile
	{
        public ColorProfile()
        {
			CreateMap<Color, ColorItemDTO>().ReverseMap();

			CreateMap<ColorCreateDTO, Color>();

			CreateMap<ColorUpdateDTO, Color>().ReverseMap();
		}
    }
}
