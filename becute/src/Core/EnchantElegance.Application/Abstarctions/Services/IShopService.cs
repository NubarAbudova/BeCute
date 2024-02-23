using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IShopService
	{
		Task<ShopDTO> GetShopItems(string? search, int? order, int? categoryId, int page, int take);
	}
}
