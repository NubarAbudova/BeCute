using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.DTOs
{
	public class ShopDTO
	{
        public int? Order { get; set; }
        public int? CategoryId { get; set; }
        public string Search { get; set; }
        public List<Product> Products { get; set; }
		public List<Category> Categories { get; set; }
        public PaginationVM<Product> Pagination { get; set; }

    }
}
