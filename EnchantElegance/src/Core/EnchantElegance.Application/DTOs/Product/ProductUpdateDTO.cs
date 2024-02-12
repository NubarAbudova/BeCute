using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EnchantElegance.Application.DTOs
{
	public class ProductUpdateDTO
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

	
		public IFormFile? MainPhoto { get; set; }

		[DataType(DataType.Upload)]
		public IFormFile? HoverPhoto { get; set; }

		[DataType(DataType.Upload)]
		public List<IFormFile>? Photos { get; set; }

		[Required(ErrorMessage = "CategoryId is required")]
		public int? CategoryId { get; set; }

		public List<Category>? Categories { get; set; } = null!;

		public List<int>? ColorIds { get; set; }

		public List<Color>? Colors { get; set; }

		public List<ProductImages>? ProductImages { get; set; }
		public List<int>? ImageIds { get; set; }
	}
}
