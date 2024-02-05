using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Application.DTOs.Sliders;
using EnchantElegance.Application.DTOs.Users;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface ISliderService
	{
		Task<ICollection<SliderItemDTO>> GetAllAsync(int page, int take);
		Task<List<string>>Create(SliderCreateDTO sliderCreateDTO);
		Task Update(int id);
		Task<List<string>> Update(int id, SliderUpdateDTO updateDTO);
		Task Delete(int id);
		Task SoftDeleteAsync(int id);

	}
}
