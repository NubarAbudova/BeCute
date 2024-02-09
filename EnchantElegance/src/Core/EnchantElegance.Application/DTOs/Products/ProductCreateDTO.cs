using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnchantElegance.Application.DTOs
{
	public class ProductCreateDTO
	{
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
        public IFormFile Photo { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public int ColorId { get; set; }
		public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

	}

}


