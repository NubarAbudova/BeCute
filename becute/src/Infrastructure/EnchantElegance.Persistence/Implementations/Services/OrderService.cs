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
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
	public class OrderService : IOrderService
	{
		public Task<List<OrderDTO>> GetOrderItems()
		{
			throw new NotImplementedException();
		}
	}
}
