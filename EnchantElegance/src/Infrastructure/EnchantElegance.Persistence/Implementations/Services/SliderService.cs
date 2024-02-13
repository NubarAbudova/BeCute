using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Utilities.Extensions;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	internal class SliderService : ISliderService
	{
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly ISliderRepository _sliderrepo;

		public SliderService(AppDbContext context, IMapper mapper, IWebHostEnvironment env,ISliderRepository sliderrepo)
		{
			_mapper = mapper;
			_env = env;
			_sliderrepo = sliderrepo;
		}
		public async Task<ItemVM<Slider>> GetAllAsync(int page, int take)
		{
			List<Slider> sliders = await _sliderrepo.GetAll().ToListAsync();
			ItemVM<Slider> slidervm = new ItemVM<Slider>
			{
				Items = sliders,
			};
			return slidervm;
		}
		public async Task<SliderCreateDTO> CreatedAsync(SliderCreateDTO dto)
		{
			return dto;
		}
		public async Task<bool> Create(SliderCreateDTO sliderCreateDTO, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			Slider slider = new Slider
			{
				Name = sliderCreateDTO.Name,
				SubTitle = sliderCreateDTO.SubTitle,
				Order = sliderCreateDTO.Order,

			};

			if (await _sliderrepo.IsExistAsync(p => p.Name == sliderCreateDTO.Name))
			{
				modelstate.AddModelError("Name", "Name already exists");
			}
			if (sliderCreateDTO.Photo != null)
			{
				if (!sliderCreateDTO.Photo.ValidateType("image/"))
				{
					modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!sliderCreateDTO.Photo.ValidateSize(600))
				{
					modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
					return false;
				}
				string fileName = await sliderCreateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");
				slider.Image = fileName;
			}

			
			await _sliderrepo.AddAsync(slider);
			await _sliderrepo.SaveChangesAsync();
			return true;
		}

		public async Task<SliderUpdateDTO> GetProductForUpdateAsync(int id, SliderUpdateDTO updateDTO)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Slider exist = await _sliderrepo.GetByIdAsync(id);

			if (exist == null) throw new Exception("Not Found");

			updateDTO.Image = exist.Image;
			updateDTO.Name = exist.Name.Trim();
			updateDTO.SubTitle = exist.SubTitle;
			updateDTO.Order= exist.Order;

			return updateDTO;
		}
		public async Task<bool> Update(int id, SliderUpdateDTO updateDTO, ModelStateDictionary modelstate)
		{
			if (id <= 0) throw new Exception("Bad Request");

			if (!modelstate.IsValid) return false;
			Slider existed = await _sliderrepo.GetByIdAsync(id);
			List<Slider> slider = await _sliderrepo.GetAll().ToListAsync();

			if (existed is null) throw new Exception("Not Found");

			if (await _sliderrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _sliderrepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Slider is already exist");
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
				string fileName = await updateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slider");
				existed.Image.DeleteFile(_env.WebRootPath, "assets", "img", "slider");

				existed.Image = fileName;
			}

			existed.Name = updateDTO.Name;
			existed.SubTitle = updateDTO.SubTitle;
			existed.Order = updateDTO.Order;


			_sliderrepo.Update(existed);
			await _sliderrepo.SaveChangesAsync();
			return true;
		}
		public async Task<bool>Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Slider exist = await _sliderrepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			Slider slider = await _sliderrepo.GetByExpressionAsync(s=>s.Id== id);

			if (exist.Image is not null)
			{
				exist.Image.DeleteFile(_env.WebRootPath, "assets", "img","slider");
			}
			_sliderrepo.Delete(exist);
			await _sliderrepo.SaveChangesAsync();
			return true;
		}

		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
	}
}
