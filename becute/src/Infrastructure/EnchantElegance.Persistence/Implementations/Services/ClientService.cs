using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using EnchantElegance.Domain.Utilities.Extensions;

using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
    public class ClientService : IClientService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IClientRepository _clientrepo;

        public ClientService(IWebHostEnvironment env, IClientRepository Clientrepo)
        {
            _env = env;
            _clientrepo = Clientrepo;
        }
        public async Task<PaginationVM<Client>> GetAllAsync(int page = 1, int take = 3)
        {
            ICollection<Client> clients = await _clientrepo.GetPagination(skip: (page - 1) * take, take: take).ToListAsync();

            int count = await _clientrepo.GetAll().CountAsync();

            double totalpage = Math.Ceiling((double)count / take);
            PaginationVM<Client> ClientVM = new PaginationVM<Client>
            {
                Items = clients,
                CurrentPage = page,
                TotalPage = totalpage
            };
            return ClientVM;
        }
        public async Task<ClientCreateDTO> CreatedAsync(ClientCreateDTO dto)
        {
            return dto;
        }
        public async Task<bool> Create(ClientCreateDTO ClientCreateDTO, ModelStateDictionary modelstate)
        {
            if (!modelstate.IsValid) return false;
            Client Client = new Client
            {
                Name = ClientCreateDTO.Name,
                Description= ClientCreateDTO.Description,
                Profession=ClientCreateDTO.Profession,
            };

            if (await _clientrepo.IsExistAsync(p => p.Name == ClientCreateDTO.Name))
            {
                modelstate.AddModelError("Name", "Name already exists");
            }
            if (ClientCreateDTO.Photo != null)
            {
                if (!ClientCreateDTO.Photo.ValidateType("image/"))
                {
                    modelstate.AddModelError("Photo", "File type does not match. Please upload a valid image.");
                    return false;
                }
                if (!ClientCreateDTO.Photo.ValidateSize(600))
                {
                    modelstate.AddModelError("Photo", "File size should not be larger than 2MB.");
                    return false;
                }

                string fileName = await ClientCreateDTO.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img");
                Client.Image = fileName;
            }

            await _clientrepo.AddAsync(Client);
            await _clientrepo.SaveChangesAsync();
            return true;
        }

        public async Task<ClientUpdateDTO> GetClientForUpdateAsync(int id, ClientUpdateDTO updateDTO)
        {
            if (id <= 0) throw new Exception("Bad Request");

            Client exist = await _clientrepo.GetByIdAsync(id);

            if (exist == null) throw new Exception("Not Found");

            updateDTO.Image = exist.Image;
            updateDTO.Name = exist.Name.Trim();
            updateDTO.Description = exist.Description.Trim();
            updateDTO.Profession= exist.Profession.Trim();

            return updateDTO;
        }
        public async Task<bool> Update(int id, ClientUpdateDTO updateDTO, ModelStateDictionary modelstate)
        {
            if (id <= 0) throw new Exception("Bad Request");

            if (!modelstate.IsValid) return false;
            Client existed = await _clientrepo.GetByIdAsync(id);
            List<Client> Client = await _clientrepo.GetAll().ToListAsync();


            if (existed is null) throw new Exception("Not Found");

            if (await _clientrepo.IsExistAsync(c => c.Name == updateDTO.Name) && await _clientrepo.IsExistAsync(c => c.Id != id))
            {
                modelstate.AddModelError("Name", "Client is already exist");
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
            _clientrepo.Update(existed);
            await _clientrepo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            if (id <= 0) throw new Exception("Bad Request");

            Client exist = await _clientrepo.GetByIdAsync(id);

            if (exist is null) throw new Exception("Not Found");

            List<Client> Client = await _clientrepo.GetAll().ToListAsync();

            if (exist.Image is not null)
            {
                exist.Image.DeleteFile(_env.WebRootPath, "assets", "img");
            }
            _clientrepo.Delete(exist);
            await _clientrepo.SaveChangesAsync();
            return true;
        }
    }
}
