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
        public List<BasketItem> BasketItems { get; set; }
    }
}
