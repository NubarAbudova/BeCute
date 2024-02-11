using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IProductService
	{
		Task<ItemVM<Product>> GetAllAsync(int page, int take);
		Task<ProductCreateDTO> CreatedAsync(ProductCreateDTO dto);
		Task <bool> Create(ProductCreateDTO productCreateDTO,ModelStateDictionary modelstate);

		//Task<ProductUpdateDTO> GetProductForUpdateAsync(int id);
		//Task Update(int id, ProductUpdateDTO updateDTO);
		//Task Delete(int id);
		//Task SoftDeleteAsync(int id);
	}
}
