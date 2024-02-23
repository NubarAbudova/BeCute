using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EnchantElegance.Application.Abstarctions.Services
{
	public interface IOrderService
	{
		Task<List<OrderDTO>> GetOrderItems();

	}
}
