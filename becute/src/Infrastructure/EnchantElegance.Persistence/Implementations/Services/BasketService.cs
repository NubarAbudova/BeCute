using System.Collections.Generic;
using System.Security.Claims;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Implementations.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;

namespace EnchantElegance.Persistence.Implementations.Services
{
    public class BasketService : IBasketService
    {
        private readonly IProductRepository _productrepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _account;
        private readonly IBasketItemRepository _basketitemrepo;

        public BasketService(IProductRepository productrepo, IHttpContextAccessor httpContextAccessor, IAccountService account, IBasketItemRepository basketitemrepo)
        {
            _productrepo = productrepo;
            _httpContextAccessor = httpContextAccessor;
            _account = account;
            _basketitemrepo = basketitemrepo;
        }
        public async Task<List<BasketItemDTO>> GetBasketItems()
        {
            List<BasketItemDTO> items = new List<BasketItemDTO>();
			AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
			ICollection<BasketItem> basketItems = await _basketitemrepo
                .GetAllWhere(x => x.AppUserId == user.Id, includes: new string[] { nameof(BasketItem.Product) })
                .ToListAsync();
			items = basketItems.Select(basketItem => new BasketItemDTO
			{
				Id = basketItem.Id,
				Price = basketItem.Price,
				Count = basketItem.Count,
                Image=basketItem.Image,
                SubTotal=basketItem.Price*basketItem.Count,
				Name = basketItem.ProductName,
				AppUserId = basketItem.AppUserId,
				ProductId = basketItem.ProductId,
			}).ToList();

			return items;

        }
        public async Task<bool> AddToBasket(int productId)
        {
            if (productId <= 0) return false;

            Product product = await _productrepo.GetByIdAsync(productId, includes: new string[] { nameof(Product.ProductImages) });

            if (product == null) return false;


            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account.GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("User not found");
                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == productId);
                if (item == null)
                {
                    item = new BasketItem
                    {
                        IsDeleted = false,
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Count = 1,
                        Price = product.CurrentPrice,
                        Image = product.ProductImages.FirstOrDefault(x => x.IsPrimary == true)?.Url,

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

        public async Task Checkout()
        {
            AppUser user = await _account
                .GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
            OrderDTO order = new OrderDTO
            {
                BasketItems = user.BasketItems,
            };
           
        }

        public async Task Remove(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");

            Product product = await _productrepo.GetByIdAsync(id);

            if (product == null) throw new Exception("Not Found");

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account
                    .GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);

                if (user == null) throw new Exception("Not Found");

                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);

                if (item != null)
                {
                    user.BasketItems.Remove(item);

                }
                await _basketitemrepo.SaveChangesAsync();
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
            if (id <= 0) throw new Exception("Bad Request");
            Product product = await _productrepo.GetByIdAsync(id);
            if (product == null) throw new Exception("Not Found");
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account
                    .GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("Not Found");

                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);
                if (item != null)
                {
                    if (item.Count > 1)
                    {

                        item.Count--;
                    }

                }

                _basketitemrepo.Update(item);
                await _basketitemrepo.SaveChangesAsync();
            }
            else
            {
                List<BasketCookieItemDTO> cookiesVM = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);
                BasketCookieItemDTO basketcookies = cookiesVM.FirstOrDefault(c => c.Id == id);
                if (basketcookies == null) throw new Exception("Not Found");
                if (basketcookies.Count > 1)
                {

                    basketcookies.Count--;
                }
                else
                {
                    cookiesVM.Remove(basketcookies);
                }
                string json = JsonConvert.SerializeObject(cookiesVM);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Basket", json);
            }

        }
        public async Task Plus(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");
            Product product = await _productrepo.GetByIdAsync(id);
            if (product == null) throw new Exception("Not Found");
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser user = await _account
                    .GetUserAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
                if (user == null) throw new Exception("Not Found");

                BasketItem item = user.BasketItems.FirstOrDefault(bi => bi.ProductId == product.Id);
                if (item != null)
                {
                    item.Count++;
                }

                _basketitemrepo.Update(item);
                await _basketitemrepo.SaveChangesAsync();
            }
            else
            {
                List<BasketCookieItemDTO> cookiesVM = JsonConvert.DeserializeObject<List<BasketCookieItemDTO>>(_httpContextAccessor.HttpContext.Request.Cookies["Basket"]);
                BasketCookieItemDTO basketcookies = cookiesVM.FirstOrDefault(c => c.Id == id);
                if (basketcookies == null) throw new Exception("Not Found");
                if (basketcookies.Count > 1)
                {
                    basketcookies.Count++;
                }
                else
                {
                    cookiesVM.Remove(basketcookies);
                }
                string json = JsonConvert.SerializeObject(cookiesVM);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("Basket", json);
            }
        }



    }
}
