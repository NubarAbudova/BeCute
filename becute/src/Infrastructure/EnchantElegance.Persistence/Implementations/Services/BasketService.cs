using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Bcpg;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class BasketService : IBasketService
	{
		private readonly IProductRepository _productrepo;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _account;
        private readonly IBasketItemRepository _basketitemrepo;

        public BasketService(IProductRepository productrepo, IHttpContextAccessor httpContextAccessor,IAccountService account,IBasketItemRepository basketitemrepo)
		{
			_productrepo = productrepo;
			_httpContextAccessor = httpContextAccessor;
            _account = account;
            _basketitemrepo = basketitemrepo;
        }
		public async Task<List<BasketItemDTO>> GetBasketItems()
		{
			List<BasketItemDTO> items = new List<BasketItemDTO>();

			if(_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
				AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
				List<BasketItem> basketitems = await _basketitemrepo.GetAllWhere(b => b.AppUserId == user.Id, includes: new string[] { nameof(BasketItem.Product) }).ToListAsync();
				items = basketitems.Select(basketitem => new BasketItemDTO
				{
					Id = basketitem.Id,
					Name=basketitem.ProductName,
					Price = basketitem.Price,
					Count= basketitem.Count,
					AppUserId=basketitem.AppUserId,
					ProductId=basketitem.ProductId,
                    Image=basketitem.Image

				}).ToList();
			}
			else
			{
                if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.Request.Cookies["Basket"] is not null)
                {
                    List<BasketCookieItemDTO> cookies = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);
                    foreach (var cookie in cookies)
                    {
                       Product product = await _productrepo.GetByIdAsync(cookie.Id);

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

		     Product product = await _productrepo.GetByIdAsync(productId,includes:new string[] {nameof(Product.ProductImages)});

			if (product == null) return false;


			if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
			{
                AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User not found");
                BasketItem item=user.BasketItems.FirstOrDefault(bi=>bi.ProductId== productId);
                if(item == null)
                {
                    item = new BasketItem
                    {
                        IsDeleted = false,
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Count = 1,
                        Price = product.CurrentPrice,
                        Image=product.ProductImages.FirstOrDefault(x=>x.IsPrimary==true)?.Url,
                        
                    };
                    user.BasketItems.Add(item);
                }
                else
                {
                    item.Count++;
                }
                await _basketitemrepo.SaveChangesAsync();
			}
			else
			{
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
            }
		

			return true;
		}
        public async Task Remove(int id)
        {
            if (id <= 0)
                throw new Exception("Bad Request");

            Product product = await _productrepo.GetByIdAsync(id, includes: new string[] { nameof(BasketItem) });

            if (product == null)
                throw new Exception("Not Found");

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                if (user == null)
                    throw new Exception("User Not Found");

                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);

                if (item != null)
                {
                    user.BasketItems.Remove(item);
                    await _basketitemrepo.SaveChangesAsync();
                }
            }
            else
            {
                List<BasketCookieItemDTO> cookiesVM = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);
                BasketCookieItemDTO basketcookies = cookiesVM.FirstOrDefault(c => c.Id == id);
                cookiesVM.Remove(basketcookies);
                string json = JsonConvert.SerializeObject(cookiesVM);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Basket", json);
            }
        }
        public async Task Minus(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Product product = await _productrepo.GetByIdAsync(id);
            if (product == null) throw new Exception("Product Not Found:(");
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User Not Found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);
                if (item == null) throw new Exception("Item Not Found:(");
                item.Count--;
                _basketitemrepo.Update(item);
                await _basketitemrepo.SaveChangesAsync();
            }
        }
        public async Task Plus(int id)
        {
            if (id <= 0) throw new Exception("Wrong querry");
            Product product = await _productrepo.GetByIdAsync(id);
            if (product == null) throw new Exception("Product Not Found:(");
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User Not Found:(");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);
                if (item == null) throw new Exception("Item Not Found:(");
                item.Count++;
                _basketitemrepo.Update(item);
                await _basketitemrepo.SaveChangesAsync();
            }
        }


    }
}
