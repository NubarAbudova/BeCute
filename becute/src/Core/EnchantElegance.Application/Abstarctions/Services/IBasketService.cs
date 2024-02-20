using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;

namespace EnchantElegance.Application.Abstarctions.Services
{
    public interface IBasketService
    {
        Task<List<BasketItemDTO>> GetBasketItems();
        Task<bool> AddToBasket(int productId);
        Task Remove(int id);
        Task Minus(int id);
        Task Plus(int id);
    }
}
