using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Services
{
    public interface IBasketItemService
    {
        //Task<ICollection<BasketItemVM>> GetBasketItems();
        Task AddBasket(int id);
        Task Remove(int id);
        Task Minus(int id);
        Task Plus(int id);
    }
}
