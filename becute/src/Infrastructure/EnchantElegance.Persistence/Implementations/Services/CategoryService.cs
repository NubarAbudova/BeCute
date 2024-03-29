﻿using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Utilities.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class CategoryService : ICategoryService
	{
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly ICategoryRepository _categoryrepo;

		public CategoryService(IMapper mapper, IWebHostEnvironment env, ICategoryRepository categoryrepo)
		{
			_mapper = mapper;
			_env = env;
			_categoryrepo = categoryrepo;
		}
        public async Task<PaginationVM<Category>> GetAllAsync(int page = 1, int take = 3)
        {
			ICollection<Category> categories = await _categoryrepo.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();

            int count = await _categoryrepo.GetAll().CountAsync();

            double totalpage = Math.Ceiling((double)count / take);
            PaginationVM<Category> categoryVM = new PaginationVM<Category>
            {
                Items = categories,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return categoryVM;
        }
        public async Task<CategoryCreateDTO> CreatedAsync(CategoryCreateDTO dto)
		{
			return dto;
		}
		public async Task<bool> Create(CategoryCreateDTO categoryCreateDTO, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			Category category = new Category
			{
				Name = categoryCreateDTO.Name,
			};

			if (await _categoryrepo.IsExistAsync(p => p.Name == categoryCreateDTO.Name))
			{
				modelstate.AddModelError("Name", "Name already exists");
			}
			if (categoryCreateDTO.Photo != null)
			{
				if (!categoryCreateDTO.Photo.ValidateType("image/"))
				{
					modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!categoryCreateDTO.Photo.ValidateSize(600))
				{
					modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
					return false;
				}

				string fileName = await categoryCreateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
				category.Image = fileName;
			}

			await _categoryrepo.AddAsync(category);
			await _categoryrepo.SaveChangesAsync();
			return true;
		}

		public async Task<CategoryUpdateDTO> GetCategoryForUpdateAsync(int id, CategoryUpdateDTO updateDTO)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Category exist = await _categoryrepo.GetByIdAsync(id);

			if (exist == null) throw new Exception("Not Found");

			updateDTO.Image = exist.Image;
			updateDTO.Name = exist.Name.Trim();

			return updateDTO;
		}
		public async Task<bool> Update(int id, CategoryUpdateDTO updateDTO, ModelStateDictionary modelstate)
		{
			if (id <= 0) throw new Exception("Bad Request");

			if (!modelstate.IsValid) return false;
			Category existed = await _categoryrepo.GetByIdAsync(id);
			List<Category> category = await _categoryrepo.GetAll().ToListAsync();	
				

			if (existed is null) throw new Exception("Not Found");

			if (await _categoryrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _categoryrepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Categor is already exist");
				return false;
			}

			if (updateDTO.Photo != null)
			{
				if (!updateDTO.Photo.ValidateType("image/"))
				{
					modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!updateDTO.Photo.ValidateSize(600))
				{
					modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
					return false;
				}
				string fileName = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img");

				existed.Image = fileName;
			}

			existed.Name = updateDTO.Name;
			_categoryrepo.Update(existed);
			await _categoryrepo.SaveChangesAsync();
			return true;
		}
		public async Task<bool> Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Category exist = await _categoryrepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			List<Category> category = await _categoryrepo.GetAll().ToListAsync();

			if (exist.Image is not null)
			{
				exist.Image.DeleteFile(_env.WebRootPath, "assets", "img");
			}
			_categoryrepo.Delete(exist);
			await _categoryrepo.SaveChangesAsync();
			return true;
		}

		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
	}

}

