using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Services
{
    public interface ICategoryService
    {
		Task<ItemVM<Category>> GetAllAsync(int page, int take);
		Task<List<string>> Create(CategoryCreateDTO CategoryCreateDTO);
		Task GetCategoryForUpdateAsync(int id);
		Task Update(int id, CategoryUpdateDTO updateDTO);
		Task Delete(int id);
		Task SoftDeleteAsync(int id);
	}
}
