using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.DTO;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServices _CartServisces;

        public CartController(ICartServices CartServisces)
        {
            _CartServisces = CartServisces;
        }
        public IActionResult Index() => View(new CartOrderViewModel { Cart = _CartServisces.GetViewModel() });

        public IActionResult Add(int id)
        {
            _CartServisces.Add(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            _CartServisces.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrement(int id)
        {
            _CartServisces.Decrement(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            _CartServisces.Clear();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _CartServisces.GetViewModel(),
                    Order = OrderModel
                });

            //var order = await OrderService.CreateOrder(
            //    User.Identity!.Name,
            //    _CartServisces.GetViewModel(),
            //    OrderModel);

            var order_model = new CreateOrderModel
            {
                Order = OrderModel,
                Items = _CartServisces.GetViewModel().Items.Select(item => new OrderItemDTO
                {
                    Price = item.Product.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
            var order = await OrderService.CreateOrder(User.Identity!.Name, order_model);

            _CartServisces.Clear();
            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();

        }
    }
}
