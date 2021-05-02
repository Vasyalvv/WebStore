using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.TestApi;

namespace WebStore.Controllers
{
    public class WebApiController : Controller
    {
        private readonly IValuesService _ValuesService;

        public WebApiController(IValuesService ValuesService) => _ValuesService = ValuesService;

        public IActionResult Index()
        {
            var values = _ValuesService.Get();
            return View(values);
        }
    }
}
