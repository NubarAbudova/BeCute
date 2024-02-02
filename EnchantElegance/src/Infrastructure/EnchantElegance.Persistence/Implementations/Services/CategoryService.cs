using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Persistence.Implementations.Services
{
    //public class CategoryService : ICategoryService
    //{
    //    private readonly ICategoryRepository _repository;
    //    private readonly IMapper _mapper;

    //    public CategoryService(ICategoryRepository repository, IMapper mapper)
    //    {
    //        _repository = repository;
    //        _mapper = mapper;
    //    }

    //    public async Task<ICollection<CategoryItemDTO>> GetAllAsync(int page, int take)
    //    {
    //        ICollection<Category> categories = await _repository.GetAllWhere(skip: (page - 1) * take, take: take).ToListAsync();

    //        return _mapper.Map<ICollection<CategoryItemDTO>>(categories);
    //    }
    //    //public async Task<GetCategoryDTO> GetAsync(int id)
    //    //{
    //    //    Category category = await _repository.GetByIdAsync(id);
    //    //    if (category == null) throw new Exception("Not found");

    //    //    return new GetCategoryDTO
    //    //    {
    //    //        Id = category.Id,
    //    //        Name = category.Name,
    //    //    };
    //    //}

    //    public async Task Create(CategoryCreateDTO categoryDTO)
    //    {
    //        await _repository.AddAsync(new Category
    //        {
    //            Name = categoryDTO.Name,
    //        });
    //        await _repository.SaveChangesAsync();
    //    }

    //    public async Task<CategoryUpdateDTO> Update(int id, string name)
    //    {
    //        Category existed = await _repository.GetByIdAsync(id);
    //        if (existed == null) throw new Exception("Not found");
    //        existed.Name = name;
    //        _repository.Update(existed);
    //        await _repository.SaveChangesAsync();
    //        return new CategoryUpdateDTO(existed.Id, existed.Name);
    //    }
    //    public async Task SoftDeleteAsync(int id)
    //    {
    //        Category category = await _repository.GetByIdAsync(id);
    //        if (category == null) throw new Exception("Connot Found");

    //        _repository.SoftDelete(category);
    //        await _repository.SaveChangesAsync();
    //    }
    //    //public async Task Delete(int id)
    //    //{
    //    //    Category existed = await _repository.GetByIdAsync(id);

    //    //    if (existed == null) throw new Exception("Not found");

    //    //    _repository.Delete(existed);
    //    //    await _repository.SaveChangesAsync();
    //    //}
    //}
}
