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
    public interface IClientService
    {
        Task<PaginationVM<Client>> GetAllAsync(int page, int take);
        Task<ClientCreateDTO> CreatedAsync(ClientCreateDTO dto);
        Task<bool> Create(ClientCreateDTO ClientCreateDTO, ModelStateDictionary modelstate);
        Task<ClientUpdateDTO> GetClientForUpdateAsync(int id, ClientUpdateDTO dto);
        Task<bool> Update(int id, ClientUpdateDTO updateDTO, ModelStateDictionary modelstate);
        Task<bool> Delete(int id);
    }
}
