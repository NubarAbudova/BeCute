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

		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		public string? Description { get; set; }

		[Required(ErrorMessage = "OldPrice is required")]
		[Range(0, double.MaxValue, ErrorMessage = "OldPrice must be greater than or equal to 0")]
		public decimal OldPrice { get; set; }

		[Required(ErrorMessage = "CurrentPrice is required")]
		[Range(0, double.MaxValue, ErrorMessage = "CurrentPrice must be greater than or equal to 0")]
		public decimal CurrentPrice { get; set; }

		[Required(ErrorMessage = "MainPhoto is required")]
		[DataType(DataType.Upload)]
		public IFormFile MainPhoto { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile HoverPhoto { get; set; }

		[DataType(DataType.Upload)]
		public List<IFormFile>? Photos { get; set; }

		[Required(ErrorMessage = "CategoryId is required")]
		public int? CategoryId { get; set; }

		public List<Category>? Categories { get; set; } = null!;

		public List<int>? ColorIds { get; set; }

		public List<Color> Colors { get; set; }
	}
}
