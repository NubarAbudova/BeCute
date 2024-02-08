using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IProductService
	{
		Task<ItemVM<Product>> GetAllAsync(int page, int take);
		Task<List<string>> Create(ProductCreateDTO ProductCreateDTO);
		Task GetProductForUpdateAsync(int id);
		Task Update(int id, ProductUpdateDTO updateDTO);
		Task Delete(int id);
		Task SoftDeleteAsync(int id);
	}
}
