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
using EnchantElegance.Application.DTOs.Sliders;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	internal class SliderService : ISliderService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;

		public SliderService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
		{
			_context = context;
			_mapper = mapper;
			_env = env;
		}
		public async Task<ItemVM<Slider>> GetAllAsync(int page, int take)
		{
			List<Slider> sliders = await _context.Sliders.ToListAsync();
			ItemVM<Slider> slidervm = new ItemVM<Slider>
			{
				Items = sliders,
			};
			return slidervm;
		}

		public async Task<List<string>> Create(SliderCreateDTO sliderDTO)
		{
			List<string> str = new List<string>();
			if (sliderDTO.Photo != null)
			{

				if (!sliderDTO.Photo.ValidateType("image/"))
				{
					str.Add("File type does not match");
					return str;
				}
				if (!sliderDTO.Photo.ValidateSize(2 * 1024))
				{
					str.Add("File size should not be larger than 2MB");
					return str;
				}
			}

			string fileName = await sliderDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");

			Slider slider = new Slider
			{
				Image = fileName,
				Name = sliderDTO.Name,
				SubTitle = sliderDTO.SubTitle,
				Description = sliderDTO.Description,
				Order = sliderDTO.Order,
			};

			await _context.Sliders.AddAsync(slider);
			await _context.SaveChangesAsync();
			return str;
		}
		public async Task GetSliderForUpdateAsync(int id)
		{
			Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (slider == null)
			{
				throw new Exception("Slider is null");

			}

			SliderUpdateDTO updateDTO = _mapper.Map<SliderUpdateDTO>(slider);
		
			await _context.SaveChangesAsync();
		}

		public async Task Update(int id, SliderUpdateDTO updateDTO)
		{
			Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (slider == null)
			{
				throw new Exception("Slider is null");
			}

			// Güncelleme işlemlerini yapın
			slider.Name = updateDTO.Name;
			slider.Description = updateDTO.Description;
			slider.SubTitle = updateDTO.SubTitle;
			slider.Order = updateDTO.Order;

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
				if (!string.IsNullOrEmpty(slider.Image))
				{
					slider.Image.DeleteFile(_env.WebRootPath, "assets", "img", "slider");
				}

				// Yeni fotoğrafı ekleyin
				slider.Image = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");
			}

			await _context.SaveChangesAsync();
		}
		public async Task Delete(int id)
		{
			Slider existed = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);

			if (existed == null)
			{
				throw new Exception("slider is null");
			}

			try
			{
				_context.Sliders.Remove(existed);
				await _context.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw new Exception("slider is null");

			}

			if (!string.IsNullOrEmpty(existed.Image))
			{
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "slider");
			}

			await _context.SaveChangesAsync();
		}


		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}

	}
}
