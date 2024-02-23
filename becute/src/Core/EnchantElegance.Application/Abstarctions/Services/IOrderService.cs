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
        Task<OrderCreateDTO> CheckOuted(OrderCreateDTO orderdto);
        Task<ICollection<OrderDTO>> AcceptOrders(string username);
        Task<ICollection<OrderDTO>> DeliveredOrders(string userName);
        Task<ICollection<OrderDTO>> GetAllOrdersByUserName(string username);
        Task<ICollection<OrderDTO>> GetAllDeliveredOrdersByUserName(string username);
        Task<OrderDTO> GetOrderById(int id);
        Task<bool> CheckOut(OrderCreateDTO orderDTO, ModelStateDictionary modelstate, ITempDataDictionary keys, string stripeEmail, string stripeToken);
        Task<OrderUpdateDTO> Updated(int id, OrderUpdateDTO dto);
        Task<bool> Update(int id, OrderUpdateDTO orderdto, ModelStateDictionary modelState);

    }
}
