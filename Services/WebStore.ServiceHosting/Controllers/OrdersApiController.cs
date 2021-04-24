using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Управление заказами</summary>
    [Route(WebAPI.Orders)]
    [ApiController]
    public class OrdersApiController : ControllerBase, IOrderService
    {
        private readonly IOrderService _OrderService;

        public OrdersApiController(IOrderService OrderService)
        {
            _OrderService = OrderService;
        }

        /// <summary>Создание нового заказа</summary>
        /// <param name="UserName">Имя пользователя, сделавшего заказ</param>
        /// <param name="OrderModel">Информация о заказе</param>
        /// <returns>Информация о сформированном заказе</returns>
        [HttpPost("{UserName}")]
        public async Task<OrderDTO> CreateOrder(string UserName, [FromBody] CreateOrderModel OrderModel) => await _OrderService.CreateOrder(UserName, OrderModel);
        
        /// <summary>Получение заказа по его идентификатору</summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Информация о заказе</returns>
        [HttpGet("{id:int}")]
        public async Task<OrderDTO> GetOrderById(int id) => await _OrderService.GetOrderById(id);

        /// <summary>Получение всех заказов выбранного пользователя</summary>
        /// <param name="UserName">Имя пользователя</param>
        /// <returns>Перечень заказов пользователя</returns>
        [HttpGet("user/{UserName}")]
        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string UserName) => await _OrderService.GetUserOrders(UserName);
    }
}
