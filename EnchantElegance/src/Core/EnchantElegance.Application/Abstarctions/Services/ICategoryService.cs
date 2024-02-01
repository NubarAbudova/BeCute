using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs.Categories;

namespace EnchantElegance.Application.Abstarctions.Services
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryItemDTO>> GetAllAsync(int page, int take);
        //Task<GetCategoryDTO> GetAsync(int id);
        Task Create(CategoryCreateDTO categoryDTO);
        //Task<CategoryUpdateDTO> Update(int id, string name);
        //Task Delete(int id);

        Task SoftDeleteAsync(int id);
    }
}
