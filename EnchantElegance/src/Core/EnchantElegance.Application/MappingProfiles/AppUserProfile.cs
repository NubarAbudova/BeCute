using AutoMapper;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.MappingProfiles
{
	internal class AppUserProfile:Profile
	{
        public AppUserProfile()
        {
            CreateMap<RegisterDTO, AppUser>();
			CreateMap<LoginDTO, AppUser>();
		}

	}
}
