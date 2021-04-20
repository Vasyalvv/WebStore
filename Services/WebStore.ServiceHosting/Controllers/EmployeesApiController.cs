using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Models;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Employees)]
    [ApiController]
    public class EmployeesApiController : ControllerBase, IEmployeesData
    {
        private readonly IEmployeesData _EmployeesData;
        private readonly ILogger<EmployeesApiController> _Logger;

        public EmployeesApiController(
            IEmployeesData EmployeesData, ILogger<EmployeesApiController> Logger)
        {
            _EmployeesData = EmployeesData;
            _Logger = Logger;
        }

        [HttpPost]
        public int Add(Employee employee)
        {
            _Logger.LogInformation($"Добавление нового сотрудника {employee}");
            return _EmployeesData.Add(employee);
        }

        [HttpPost("{employee}")] //post -> http://localhost:5001/api/employees/employee?LastName=Сергеев&FirstName=Александр&Patronymic=Петрович&Age=22
        public Employee Add(string LastName, string FirstName, string Patronymic, int Age)
        {
            _Logger.LogInformation($"Добавление нового сотрудника {LastName} {FirstName} {Patronymic} {Age} лет");
            return _EmployeesData.Add(LastName, FirstName, Patronymic, Age);
        }

        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            _Logger.LogInformation($"Удаление сотрудника id:{id}...");
            var result = _EmployeesData.Delete(id);
            _Logger.LogInformation("Удаление сотрудника id:{0} - {1}", id, result? "выполнено":"не найден");

            return result;
        }

        [HttpGet]   //http://localhost:5001/api/employees
        public IEnumerable<Employee> Get() => _EmployeesData.Get();

        [HttpGet("{id}")]   //http://localhost:5001/api/employees/77
        public Employee Get(int id) => _EmployeesData.Get(id);

        [HttpGet("employee")] //http://localhost:5001/api/employees/employee?LastName=Сергеев&FirstName=Александр&Patronymic=Петрович
        public Employee GetByName(string LastName, string FirstName, string Patronymic) 
            => _EmployeesData.GetByName(LastName, FirstName, Patronymic);

        [HttpPut]  //put -> http://localhost:5001/api/employees/77
        public void Update(Employee employee)
        {
            _Logger.LogInformation($"Редактирование сотрудника {employee}");
            _EmployeesData.Update(employee);
        }
    }
}
