﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        [HttpPost("{UserName}")]
        public async Task<OrderDTO> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel) => await _OrderService.CreateOrder(UserName, OrderModel);
        [HttpGet("{id:int}")]
        public async Task<OrderDTO> GetOrderById(int id) => await _OrderService.GetOrderById(id);

        [HttpGet("user/{UserName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) => await _OrderService.GetUserOrders(UserName);
    }
}
