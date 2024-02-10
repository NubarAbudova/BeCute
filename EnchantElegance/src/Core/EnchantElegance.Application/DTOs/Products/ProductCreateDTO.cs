using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EnchantElegance.Application.DTOs
{

	public class ProductCreateDTO
	{
		[Required(ErrorMessage = "Product name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Product description is required.")]
		public string Description { get; set; }

		[Required(ErrorMessage = "Product price should be greater than zero.")]
		[Range(0.01, double.MaxValue, ErrorMessage = "Please enter a value greater than zero.")]
		public decimal CurrentPrice { get; set; }

		public decimal OldPrice { get; set; }

		public IFormFile MainPhoto { get; set; }

		public IFormFile HoverPhoto { get; set; }
        public List<IFormFile> Photos { get; set; }

        [Required(ErrorMessage = "Category is required.")]
		public int CategoryId { get; set; }
		public List<Category> Categories { get; set; }
		public List<int> ColorIds { get; set; }
		public List<Color> Colors { get; set; }
	}



}


