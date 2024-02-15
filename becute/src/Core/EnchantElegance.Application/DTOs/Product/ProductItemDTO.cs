using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.DTOs
{
	public class ProductItemDTO
	{
        public string Name { get; set; }
        public string? Description { get; set; }
		public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
		public string? Color { get; set; }
		public int CategoryId { get; set; }
		public Category Category { get; set; } = null!;
		public ICollection<ProductImages> ProductImages { get; set; } = null!;
		public ICollection<ProductColor> ProductColors { get; set; }
	}
}
