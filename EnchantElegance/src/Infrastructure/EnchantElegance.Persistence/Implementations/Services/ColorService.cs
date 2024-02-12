using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Persistence.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class ColorService : IColorService
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;
		private readonly IWebHostEnvironment _env;
		private readonly IColorRepository _colorrepo;

		public ColorService(AppDbContext context, IMapper mapper, IWebHostEnvironment env,IColorRepository colorrepo)
        {
			_context = context;
			_mapper = mapper;
			_env = env;
			_colorrepo = colorrepo;
		}
		public async Task<ItemVM<Color>> GetAllAsync(int page, int take)
		{
			List<Color> colors = await _context.Colors.ToListAsync();
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
			await _context.Colors.AddAsync(color);
			await _context.SaveChangesAsync();
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

			Color color = await _colorrepo.GetByIdAsync(id);

			Color existed = await _context.Colors
				.FirstOrDefaultAsync(p => p.Id == id);

			if (existed is null) throw new Exception("Not Found");

			if (await _colorrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _colorrepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Color is already exist");
				return false;
			}
	
			existed.Name = updateDTO.Name;

			_colorrepo.Update(color);
			await _colorrepo.SaveChangesAsync();
			return true;
		}

		public async Task<bool> Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Color exist = await _colorrepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			Category category = await _context.Categories.FirstOrDefaultAsync(s => s.Id == id);

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
