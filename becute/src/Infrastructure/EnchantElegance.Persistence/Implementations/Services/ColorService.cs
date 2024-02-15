using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
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
		public async Task<PaginationVM<Color>> GetAllAsync(int page = 1, int take = 3)
		{
			ICollection<Color> colors = await _colorrepo.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();

			int count = await _colorrepo.GetAll().CountAsync();

			double totalpage = Math.Ceiling((double)count / take);
			PaginationVM<Color> colorVM = new PaginationVM<Color>
			{
				Items = colors,
				CurrentPage = page,
				TotalPage = totalpage
			};
			return colorVM;
		}
		public async Task<ColorCreateDTO> CreatedAsync(ColorCreateDTO dto)
		{
			return dto;
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
				No= colorCreateDTO.No
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
			updateDTO.No = exist.No;

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
			existed.No = updateDTO.No;


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
