using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace EnchantElegance.Application.DTOs
{
	public class ProductCreateDTO
	{
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal OldPrice { get; set; }
		public decimal CurrentPrice { get; set; }
		public IFormFile MainPhoto { get; set; }
		public IFormFile HoverPhoto { get; set; }
		public List<IFormFile>? Photos { get; set; }
		public int? CategoryId { get; set; }
		public List<Category>?Categories { get; set; } 
		public List<int>? ColorIds { get; set; }
		public List<Color>? Colors { get; set; } 
	}
}
