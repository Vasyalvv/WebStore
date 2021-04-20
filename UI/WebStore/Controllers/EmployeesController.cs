using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    //[Route("Staff")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _EmployeesData;
        public EmployeesController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        //[Route("All")]
        public IActionResult Index() => View(_EmployeesData.Get());

        //[Route("info-(id-{id})")]
        public IActionResult Details(int id)
        {
            var employee = _EmployeesData.Get(id);
            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());
            var employee = _EmployeesData.Get((int)id);
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

        [Authorize(Roles = Role.Administrators)]
        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            //Комбинированная проверка данных
            if (model.LastName == "Иванов" && model.Age < 20)
                ModelState.AddModelError("", "Неправильный сотрудник!");

            if (!ModelState.IsValid) return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                LastName = model.LastName,
                FirstName = model.Name,
                Patronymic = model.Patronymic,
                Age = model.Age
            };

            if (employee.Id == 0)
                _EmployeesData.Add(employee);
            else
                _EmployeesData.Update(employee);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrators)]
        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = _EmployeesData.Get(id);
            if (employee is null) return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                Name = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }

        [Authorize(Roles = Role.Administrators)]
        [HttpPost]  
        public IActionResult DeleteConfirmed(int id)
        {
            _EmployeesData.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
