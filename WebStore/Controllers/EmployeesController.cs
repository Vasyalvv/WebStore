using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Services.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

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


        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            var employee = _EmployeeData.Get((int)id);
            if (employee is null)
                return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Age = employee.Age,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic
            });
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age
            };

            if (employee.Id == 0)
                _EmployeeData.Add(employee);
            else
                _EmployeeData.Update(employee);


            return RedirectToAction("Index");
        }

    }
}
