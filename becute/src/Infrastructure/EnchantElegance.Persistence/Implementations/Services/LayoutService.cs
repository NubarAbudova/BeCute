using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class LayoutService
	{
		private readonly AppDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IProductRepository _productrepo;

		public LayoutService(AppDbContext context,IHttpContextAccessor http,IProductRepository productrepo)
        {
			_context = context;
			_httpContextAccessor = http;
			_productrepo = productrepo;
		}
		public async Task<Dictionary<string, string>> GetSettingsAsync()
		{
			Dictionary<string, string> settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);
			return settings;
		}
		public async Task<List<BasketItemDTO>> GetBasketAsync()
		{
			List<BasketItemDTO> items = new List<BasketItemDTO>();
			if (_httpContextAccessor.HttpContext != null)
			{
				if (_httpContextAccessor.HttpContext.Request.Cookies["Basket"] is not null)
				{
					List<BasketCookieItemDTO> cookies = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);
					foreach (var cookie in cookies)
					{
						Domain.Entities.Product product = await _productrepo.GetByIdAsync(cookie.Id);

						if (product != null)
						{
							BasketItemDTO item = new BasketItemDTO()
							{
								Id = product.Id,
								Name = product.Name,
								Image = product.ProductImages.FirstOrDefault()?.Url,
								Count = cookie.Count,
								SubTotal = product.CurrentPrice * cookie.Count,
							};
							items.Add(item);
						}
					}
				}
			}

			return items;
		}
	}
}
