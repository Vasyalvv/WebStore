using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public EmployeesApiController(IEmployeesData EmployeesData)
        {
            _EmployeesData = EmployeesData;
        }

        [HttpPost]
        public int Add(Employee employee) => _EmployeesData.Add(employee);

        [HttpPost("{employee}")] //post -> http://localhost:5001/api/employees/employee?LastName=Сергеев&FirstName=Александр&Patronymic=Петрович&Age=22
        public Employee Add(string LastName, string FirstName, string Patronymic, int Age)
            => _EmployeesData.Add(LastName, FirstName, Patronymic, Age);

        [HttpDelete("{id}")]
        public bool Delete(int id) => _EmployeesData.Delete(id);

        [HttpGet]   //http://localhost:5001/api/employees
        public IEnumerable<Employee> Get() => _EmployeesData.Get();

        [HttpGet("{id}")]   //http://localhost:5001/api/employees/77
        public Employee Get(int id) => _EmployeesData.Get(id);

        [HttpGet("{employee}")] //http://localhost:5001/api/employees/employee?LastName=Сергеев&FirstName=Александр&Patronymic=Петрович
        public Employee GetByName(string LastName, string FirstName, string Patronymic) 
            => _EmployeesData.GetByName(LastName, FirstName, Patronymic);

        [HttpPut]  //put -> http://localhost:5001/api/employees/77
        public void Update(Employee employee) => _EmployeesData.Update(employee);
    }
}
