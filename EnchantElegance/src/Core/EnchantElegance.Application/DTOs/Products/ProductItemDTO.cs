using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Application.DTOs
{
	public class ProductItemDTO
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
		public int CategoryId { get; set; }
		public int ColorId { get; set; }
	}
}
