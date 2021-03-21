using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Infrastructure.Services.Interfaces
{
    interface IEmployeesData
    {
        IEnumerable<Employee> Get();

        Employee Get(int id);

        Employee GetByName(string LastName, string FirstName, string Patronymic);

        int Add(Employee employee);

        Employee Add(string LastName, string FirstName, string Patronymic, int Age);

        void Update(Employee employee);

        bool Delete(int id);

    }
}
