using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.DTOs.Categories;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;


namespace EnchantElegance.Persistence.Implementations.Services
{

	public class CategoryService : ICategoryService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;

		public CategoryService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
		}
		public async Task<ItemVM<Category>> GetAllAsync(int page, int take)
		{
			List<Category> categories = await _context.Categories.ToListAsync();
			ItemVM<Category> categoryvm = new ItemVM<Category>
			{
				Items = categories,
			};
			return categoryvm;
		}
		public async Task<List<string>> Create(CategoryCreateDTO CategoryDTO)
		{
			List<string> str = new List<string>();
			if (CategoryDTO.Photo != null)
			{
				if (!CategoryDTO.Photo.ValidateType("image/"))
				{
					str.Add("File type does not match");
					return str;
				}
				if (!CategoryDTO.Photo.ValidateSize(2 * 1024))
				{
					str.Add("File size should not be larger than 2MB");
					return str;
				}
			}

			string fileName = await CategoryDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");

			Category category = new Category
			{
				Image = fileName,
				Name = CategoryDTO.Name,

				Description = CategoryDTO.Description,
			};

			await _context.Categories.AddAsync(category);
			await _context.SaveChangesAsync();
			return str;
		}
		public async Task GetCategoryForUpdateAsync(int id)
		{
			Category Category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

			if (Category == null)
			{
				throw new Exception("Category is null");

			}

			CategoryUpdateDTO updateDTO = _mapper.Map<CategoryUpdateDTO>(Category);

			await _context.SaveChangesAsync();
		}

		public async Task Update(int id, CategoryUpdateDTO updateDTO)
		{
			Category Category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

			if (Category == null)
			{
				throw new Exception("Category is null");
			}

			// Güncelleme işlemlerini yapın
			Category.Name = updateDTO.Name;
			Category.Description = updateDTO.Description;

			if (updateDTO.Photo != null)
			{
				// Yeni fotoğraf varsa işlemleri gerçekleştirin
				if (!updateDTO.Photo.ValidateType("image/"))
				{
					throw new Exception("File type does not match");
				}

				if (!updateDTO.Photo.ValidateSize(2 * 1024))
				{
					throw new Exception("File size should not be larger than 2MB");
				}

				// Eski fotoğraf varsa silin
				if (!string.IsNullOrEmpty(Category.Image))
				{
					Category.Image.DeleteFile(_env.WebRootPath, "assets", "img");
				}

				// Yeni fotoğrafı ekleyin
				Category.Image = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
			}

			await _context.SaveChangesAsync();
		}
		public async Task Delete(int id)
		{
			Category existed = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

			if (existed == null)
			{
				throw new Exception("Category is null");
			}

			try
			{
				_context.Categories.Remove(existed);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw new Exception("Category is null");

			}

			if (!string.IsNullOrEmpty(existed.Image))
			{
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");
			}

			await _context.SaveChangesAsync();
		}


		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}


	}


}
