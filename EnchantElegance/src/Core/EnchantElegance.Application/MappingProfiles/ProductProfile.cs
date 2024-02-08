using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.MappingProfiles
{
	internal class ProductProfile:Profile
	{
        public ProductProfile()
        {
			CreateMap<Product, ProductItemDTO>().ReverseMap();

			CreateMap<ProductCreateDTO, Product>();
			CreateMap<ProductUpdateDTO, Product>().ReverseMap();
		}
    }
}
