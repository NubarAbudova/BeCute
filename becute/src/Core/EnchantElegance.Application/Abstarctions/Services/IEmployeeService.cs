using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IEmployeeService
	{
		Task<PaginationVM<Employee>> GetAllAsync(int page, int take);
		Task<EmployeeCreateDTO> CreatedAsync(EmployeeCreateDTO dto);
		Task<bool> Create(EmployeeCreateDTO EmployeeCreateDTO, ModelStateDictionary modelstate);
		Task<EmployeeUpdateDTO> GetEmployeeForUpdateAsync(int id, EmployeeUpdateDTO dto);
		Task<bool> Update(int id, EmployeeUpdateDTO updateDTO, ModelStateDictionary modelstate);
		Task<bool> Delete(int id);
	}
}
