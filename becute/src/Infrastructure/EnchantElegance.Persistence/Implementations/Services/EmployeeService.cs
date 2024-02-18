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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using EnchantElegance.Domain.Utilities.Extensions;


namespace EnchantElegance.Persistence.Implementations.Services
{
	public class EmployeeService : IEmployeeService
	{
		private readonly IWebHostEnvironment _env;
		private readonly IEmployeeRepository _employeerepo;

		public EmployeeService(IWebHostEnvironment env, IEmployeeRepository Employeerepo)
		{
			_env = env;
			_employeerepo = Employeerepo;
		}
		public async Task<PaginationVM<Employee>> GetAllAsync(int page = 1, int take = 3)
		{
			ICollection<Employee> Employees = await _employeerepo.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();

			int count = await _employeerepo.GetAll().CountAsync();

			double totalpage = Math.Ceiling((double)count / take);
			PaginationVM<Employee> EmployeeVM = new PaginationVM<Employee>
			{
				Items = Employees,
				CurrentPage = page,
				TotalPage = totalpage
			};
			return EmployeeVM;
		}
		public async Task<EmployeeCreateDTO> CreatedAsync(EmployeeCreateDTO dto)
		{
			return dto;
		}
		public async Task<bool> Create(EmployeeCreateDTO EmployeeCreateDTO, ModelStateDictionary modelstate)
		{
			if (!modelstate.IsValid) return false;
			Employee Employee = new Employee
			{
				Name = EmployeeCreateDTO.Name,
				Profession = EmployeeCreateDTO.Profession,
			};

			if (await _employeerepo.IsExistAsync(p => p.Name == EmployeeCreateDTO.Name))
			{
				modelstate.AddModelError("Name", "Name already exists");
			}
			if (EmployeeCreateDTO.Photo != null)
			{
				if (!EmployeeCreateDTO.Photo.ValidateType("image/"))
				{
					modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
					return false;
				}
				if (!EmployeeCreateDTO.Photo.ValidateSize(600))
				{
					modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
					return false;
				}

				string fileName = await EmployeeCreateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
				Employee.Image = fileName;
			}

			await _employeerepo.AddAsync(Employee);
			await _employeerepo.SaveChangesAsync();
			return true;
		}

		public async Task<EmployeeUpdateDTO> GetEmployeeForUpdateAsync(int id, EmployeeUpdateDTO updateDTO)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Employee exist = await _employeerepo.GetByIdAsync(id);

			if (exist == null) throw new Exception("Not Found");

			updateDTO.Image = exist.Image;
			updateDTO.Name = exist.Name.Trim();
			updateDTO.Profession = exist.Profession.Trim();

			return updateDTO;
		}
		public async Task<bool> Update(int id, EmployeeUpdateDTO updateDTO, ModelStateDictionary modelstate)
		{
			if (id <= 0) throw new Exception("Bad Request");

			if (!modelstate.IsValid) return false;
			Employee existed = await _employeerepo.GetByIdAsync(id);
			List<Employee> Employee = await _employeerepo.GetAll().ToListAsync();


			if (existed is null) throw new Exception("Not Found");

			if (await _employeerepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _employeerepo.IsExistAsync(c => c.Id != id))
			{
				modelstate.AddModelError("Name", "Employee is already exist");
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
			_employeerepo.Update(existed);
			await _employeerepo.SaveChangesAsync();
			return true;
		}
		public async Task<bool> Delete(int id)
		{
			if (id <= 0) throw new Exception("Bad Request");

			Employee exist = await _employeerepo.GetByIdAsync(id);

			if (exist is null) throw new Exception("Not Found");

			List<Employee> Employee = await _employeerepo.GetAll().ToListAsync();

			if (exist.Image is not null)
			{
				exist.Image.DeleteFile(_env.WebRootPath, "assets", "img");
			}
			_employeerepo.Delete(exist);
			await _employeerepo.SaveChangesAsync();
			return true;
		}
	}
}
