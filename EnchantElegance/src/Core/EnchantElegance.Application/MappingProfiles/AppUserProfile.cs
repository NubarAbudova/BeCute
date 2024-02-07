using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.DTOs.Users;
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
