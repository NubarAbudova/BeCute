using AutoMapper;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.MappingProfiles
{
	internal class SliderProfile:Profile
	{
		public SliderProfile()
		{
			CreateMap<Slider, SliderItemDTO>().ReverseMap();

			CreateMap<SliderCreateDTO, Slider>();

			CreateMap<SliderUpdateDTO, Slider>().ReverseMap();
		}
	}
}
