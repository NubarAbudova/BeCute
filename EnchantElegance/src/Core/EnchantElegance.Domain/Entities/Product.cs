using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Domain.Entities
{
    public class Product:BaseNameableEntity
    {
        //Common Properties
        public string? Description { get; set; }
		public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
        public string? Color { get; set; }

        //Relational Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
		public ICollection<ProductImages> ProductImages { get; set; } = null!;
		public ICollection<ProductColor> ProductColors { get; set; }


	}
}
