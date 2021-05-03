using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Filters;

namespace WebStore.Controllers
{
    [ActionDescriptionAttribute("Главный контроллер")]
    public class HomeController : Controller
    {
       [ActionDescriptionAttribute("Основное действие")]
       [AddHeader("Test","HeaderValue")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SecondAction(string id)
        {
            return Content($"SecondAction\nid:{id}");
        }


        public IActionResult NotFound404()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogSingle()
        {
            return View();
        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ProductDetails()
        {
            return View();
        }

        public IActionResult Shop()
        {
            return View();
        }

        public IActionResult Throw() => throw new ApplicationException("Test error))");

        public IActionResult Error404() => View();

        public IActionResult ErrorStatus(string code) => code switch
        {
            "404" => RedirectToAction(nameof(Error404)),
            _ => Content($"Errore code:{code}")
        };
    }
}
