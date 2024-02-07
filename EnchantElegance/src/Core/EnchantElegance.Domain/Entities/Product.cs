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
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string Brand { get; set; } = null!;
        public string? Image { get; set; }

        // Cosmetics and SkinCare Properties
        public string SkinType { get; set; } = null!;
        public string SkinTone { get; set; } = null!;
        public string? Color { get; set; }
        public string Usage { get; set; } = null!;
        public string Ingredients { get; set; } = null!;
        public bool IsOrganic { get; set; }

        //Relational Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<ProductColor> ProductColors { get; set; } = null!;


    }
}
