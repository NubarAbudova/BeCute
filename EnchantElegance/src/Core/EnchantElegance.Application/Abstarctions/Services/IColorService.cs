using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IColorService
	{
		Task<ItemVM<Color>> GetAllAsync(int page, int take);
		//Task<ColorCreateDTO> CreatedAsync(ColorCreateDTO dto);
		Task<bool> Create(ColorCreateDTO colorCreateDTO, ModelStateDictionary modelstate);
		Task<ColorUpdateDTO> GetColorForUpdateAsync(int id, ColorUpdateDTO updateDTO);
		Task<bool> Update(int id, ColorUpdateDTO updateDTO, ModelStateDictionary modelstate);
		Task<bool> Delete(int id);
		Task SoftDeleteAsync(int id);
	}
}
