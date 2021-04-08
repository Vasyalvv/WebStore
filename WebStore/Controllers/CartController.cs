using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Services.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartServices _CartServisces;

        public CartController(ICartServices CartServisces)
        {
            _CartServisces = CartServisces;
        }
        public IActionResult Index() => View(_CartServisces.GetViewModel());

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
    }
}
