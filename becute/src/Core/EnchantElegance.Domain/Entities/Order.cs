using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }
        public decimal TotalPrice { get; set; }

        public List<BasketItem> BasketItems { get; set; }
        public DateTime PurchasedAt { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
