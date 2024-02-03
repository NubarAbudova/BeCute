using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantElegance.Application.DTOs.Products
{
	public record ProductCreateDTO
	(
	  string Name,
	  string? Description,
	  decimal Price,
	  int StockQuantity,
	  string Brand,
	  string SkinType,
	  string SkinTone,
	  string? Color,
	  string Usage,
	  string Ingredients,
	  bool IsOrganic,
	  
	  int CategoryId,
	  
	  ICollection<int>? ColorIds

	);
}
