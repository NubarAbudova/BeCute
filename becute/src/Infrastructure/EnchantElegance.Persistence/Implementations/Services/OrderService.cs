using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EnchantElegance.Application.Abstarctions.Repositories;
using EnchantElegance.Application.Abstarctions.Services;
using EnchantElegance.Application.DTOs;
using EnchantElegance.Application.ViewModels;
using EnchantElegance.Domain.Entities;
using EnchantElegance.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace EnchantElegance.Persistence.Implementations.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderepo;
        private readonly IHttpContextAccessor _accessor;
        private readonly IAccountService _account;
        private readonly IProductRepository _productrepo;
        private readonly IMailService _service;

        public OrderService(IOrderRepository orderepo, IHttpContextAccessor accessor,
            IAccountService account, IProductRepository productrepo, IMailService service)
        {
            _orderepo = orderepo;
            _accessor = accessor;
            _account = account;
            _productrepo = productrepo;
            _service = service;
        }

        public Task<ICollection<OrderDTO>> AcceptOrders(string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckOut(OrderCreateDTO orderDTO, ModelStateDictionary modelstate, ITempDataDictionary keys, string stripeEmail, string stripeToken)
        {
            throw new NotImplementedException();
        }

        public Task<OrderCreateDTO> CheckOuted(OrderCreateDTO orderdto)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderDTO>> DeliveredOrders(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderDTO>> GetAllDeliveredOrdersByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<OrderDTO>> GetAllOrdersByUserName(string username)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDTO> GetOrderById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(int id, OrderUpdateDTO orderdto, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }

        public Task<OrderUpdateDTO> Updated(int id, OrderUpdateDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}
