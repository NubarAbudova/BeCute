using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.DTOs
{
    public class OrderCreateDTO
    {
        public string Email { get; set; }
        public string UserName { get; set; }

        public string Address { get; set; }
        public string AddressDescription { get; set; }

        public string TownorCity { get; set; }
        public string PostalCode { get; set; }
        public bool SaveInformation { get; set; }
        public string Notes { get; set; }

        public List<BasketItem>? BasketItems { get; set; }
    }
}
