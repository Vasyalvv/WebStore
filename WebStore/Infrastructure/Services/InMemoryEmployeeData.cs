using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastructure.Services.Interfaces;
using WebStore.Models;

namespace WebStore.Infrastructure.Services
{
    public class InMemoryEmployeeData : IEmployeesData
    {
        private readonly List<Employee> _Employees;
        private int _CurrentMaxId;


        public InMemoryEmployeeData()
        {
            _Employees = TestData.Employees;
            _CurrentMaxId = _Employees.DefaultIfEmpty().Max(e => e?.Id ?? 1);

        }

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee)) return employee.Id; //С БД это нужно удалить
            employee.Id = ++_CurrentMaxId;
            _Employees.Add(employee);
            return employee.Id;
        }

        Employee Add(string LastName, string FirstName, string Patronymic,int Age)
        {
            var employee = new Employee
            {
                LastName = LastName,
                FirstName = FirstName,
                Patronymic = Patronymic,
                Age = Age
            };
            Add(employee);
            return employee; 

        }

        public bool Delete(int id)
        {
            var db_item = Get(id);
            if (db_item is null) return false;

            return _Employees.Remove(db_item);
        }

        public IEnumerable<Employee> Get()
        {
            return _Employees;
        }

        public Employee Get(int id)
        {
            return _Employees.FirstOrDefault(e => e.Id == id);
        }

        Employee GetByName(string LastName, string FirstName, string Patronymic) =>
            _Employees.FirstOrDefault(e => e.LastName == LastName && e.FirstName == FirstName && e.Patronymic == Patronymic);

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_Employees.Contains(employee)) return; //С БД это нужно удалить

            var db_item = Get(employee.Id);
            if (db_item is null) return;

            db_item.LastName = employee.LastName;
            db_item.FirstName = employee.FirstName;
            db_item.Patronymic = employee.Patronymic;
            db_item.Age = employee.Age;

            //db.SaveChanges(); //С БД
        }
    }
}
