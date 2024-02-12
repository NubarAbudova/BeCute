using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
    public interface ICategoryService
    {
		Task<ItemVM<Category>> GetAllAsync(int page, int take);
		//Task<SliderCreateDTO> CreatedAsync(ProductCreateDTO dto);
		Task<bool> Create(CategoryCreateDTO categoryCreateDTO, ModelStateDictionary modelstate);
		Task<CategoryUpdateDTO> GetCategoryForUpdateAsync(int id, CategoryUpdateDTO dto);
		Task<bool> Update(int id, CategoryUpdateDTO updateDTO, ModelStateDictionary modelstate);
		Task<bool> Delete(int id);
		Task SoftDeleteAsync(int id);
	}
}
