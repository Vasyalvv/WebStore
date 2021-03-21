using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Services.Interfaces;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeeData;
        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeeData = EmployeesData;
        }

        //[Route("All")]
        public IActionResult Index() => View(_EmployeeData.Get());

        //[Route("info-(id-{id})")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeeData.Get(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

    }
}
