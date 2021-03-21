using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    public class EmployeesController : Controller
    {
        private readonly List<Employee> _Employees;
        public EmployeesController()
        {
            _Employees = TestData.__Employees;
        }

        //[Route("All")]
        public IActionResult Index() => View(_Employees);

        //[Route("info-(id-{id})")]
        public IActionResult Details(int id)
        {
            var employee = _Employees.FirstOrDefault(e => e.Id == id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

    }
}
