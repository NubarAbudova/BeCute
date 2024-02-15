using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface ISliderService
	{
		Task<PaginationVM<Slider>> GetAllAsync(int page, int take);
		Task<SliderCreateDTO> CreatedAsync(SliderCreateDTO dto);
		Task<bool> Create(SliderCreateDTO sliderCreateDTO, ModelStateDictionary modelstate);
		Task<SliderUpdateDTO> GetProductForUpdateAsync(int id,SliderUpdateDTO dto);
		Task<bool> Update(int id, SliderUpdateDTO updateDTO, ModelStateDictionary modelstate);
		Task<bool> Delete(int id);
		Task SoftDeleteAsync(int id);
	}
}
