using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ColorService : IColorService
	{
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IColorRepository _colorrepo;

		public ColorService( IMapper mapper, IWebHostEnvironment env,IColorRepository colorrepo)
        {
			_mapper = mapper;
			_env = env;
			_colorrepo = colorrepo;
		}
		public async Task<ItemVM<Color>> GetAllAsync(int page, int take)
		{
			List<Color> colors = await _colorrepo.GetAll().ToListAsync();
			ItemVM<Color> colorvm = new ItemVM<Color>
			{
				Items = colors,
			};
			return colorvm;
		}

		public async Task<bool> Create(ColorCreateDTO colorCreateDTO, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;

			if (await _colorrepo.IsExistAsync(p => p.Name == colorCreateDTO.Name))
			{
				modelstate.AddModelError("Name", "Name already exists");
			}
			Color color = new Color
			{
				Name = colorCreateDTO.Name,
			};
			await _colorrepo.AddAsync(color);
			await _colorrepo.SaveChangesAsync();
			return true;
		}
		public async Task<ColorUpdateDTO> GetColorForUpdateAsync(int id,ColorUpdateDTO updateDTO)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Color exist = await _colorrepo.GetByIdAsync(id);

			if (exist == null) throw new Exception("Not Found");

			updateDTO.Name = exist.Name.Trim();

			return updateDTO;
		}
		public async Task<bool> Update(int id, ColorUpdateDTO updateDTO, ModelStateDictionary modelstate)
		{
			if (id <= 0) throw new Exception("Bad Request");

			if (!modelstate.IsValid) return false;

			Color existed = await _colorrepo.GetByIdAsync(id);

			List<Color> colors = await _colorrepo.GetAll().ToListAsync();

			if (existed is null) throw new Exception("Not Found");

			if (await _colorrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _colorrepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Color is already exist");
				return false;
			}
	
			existed.Name = updateDTO.Name;

			_colorrepo.Update(existed);
			await _colorrepo.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Color exist = await _colorrepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			List<Color> colors = await _colorrepo.GetAll().ToListAsync();

			_colorrepo.Delete(exist);
			await _colorrepo.SaveChangesAsync();
			return true;
		}
		public Task SoftDeleteAsync(int id)
		{
			throw new NotImplementedException();
		}
		
	}
}
