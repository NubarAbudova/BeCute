using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
    public class BasketItem:BaseEntity
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int? OrderId { get; set; }
        public Order? Order { get; set; }

    }
}
