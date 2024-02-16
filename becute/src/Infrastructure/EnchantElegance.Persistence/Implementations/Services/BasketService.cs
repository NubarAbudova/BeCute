using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EnchantElegance.Persistence.Implementations.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productrepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BasketService(IProductRepository productrepo, IHttpContextAccessor httpContextAccessor)
        {
            _productrepo = productrepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<BasketItemDTO>> GetBasketItems()
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
        public async Task<bool> AddToBasket(int productId)
        {
            if (productId <= 0) return false;

			Domain.Entities.Product product = await _productrepo.GetByIdAsync(productId);

            if (product == null) return false;

			List<BasketCookieItemDTO> basket;

            if (_httpContextAccessor.HttpContext.Request.Cookies["Basket"] is not null)
            {
                basket = new List<BasketCookieItemDTO>();
                BasketCookieItemDTO item = new BasketCookieItemDTO
                {
                    Id = productId,
                    Count = 1
                };
                basket.Add(item);
            }
            else
            {
                basket = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);

                BasketCookieItemDTO existed = basket.FirstOrDefault(b => b.Id == productId);
                if (existed == null)
                {
                    BasketCookieItemDTO item = new BasketCookieItemDTO
                    {
                        Id = productId,
                        Count = 1
                    };
                    basket.Add(item);
                }
                else
                {
                    existed.Count++;
                }
            }

            string json = JsonConvert.SerializeObject(basket);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("Basket", json);

            return true;
        }

        
    }
}
