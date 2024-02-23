using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ShopService : IShopService
	{
		private readonly IProductRepository _productrepo;
		private readonly ICategoryRepository _categoryrepo;

		public ShopService(IProductRepository productrepo, ICategoryRepository categoryrepo)
		{
			_productrepo = productrepo;
			_categoryrepo = categoryrepo;
		}
		public async Task<ShopDTO> GetShopItems(string? search, int? order, int? categoryId, int page = 1, int take = 3)
		{
			IQueryable<Product> query = _productrepo.GetAll(includes: new string[] { nameof(ProductImages) }).AsQueryable();

			switch (order)
			{
				case 1:
					query = query.OrderBy(p => p.Name);
					break;
				case 2:
					query = query.OrderBy(p => p.CurrentPrice);
					break;
				case 3:
					query = query.OrderBy(p => p.CategoryId);
					break;
				case 4:
					query = query.OrderByDescending(p => p.CreatedAt);
					break;
			}

			if (!String.IsNullOrEmpty(search))
			{
				query = query.Where(p => p.Name.ToLower().Contains(search.ToLower()));
			}

			if (categoryId != null)
			{
				query = query.Where(p => p.CategoryId == categoryId);
			}

			var totalItems = await query.CountAsync();
			var totalPages = (int)Math.Ceiling((double)totalItems / take);

			query = query.Skip((page - 1) * take).Take(take);

			ShopDTO shopDTO = new ShopDTO()
			{
				Categories = await _categoryrepo.GetAll(includes:new string[] { nameof(Category.Products) }).ToListAsync(),
				Products = await query.ToListAsync(),
				CategoryId = categoryId,
				Order = order,
				Search = search,

				Pagination = new PaginationVM<Product>
				{
					CurrentPage = page,
					TotalPage = totalPages,
					Items = query.ToList()
				}
			};

			return shopDTO;
		}
	}
}