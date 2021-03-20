using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    internal static  class TestData
    {
        public static List<Employee> __Employees { get; } = new()
        {
            new Employee() { Id = 1, Age = 20, FirstName = "Иван", LastName = "Иванов", Patronymic = "Иванович", Department = "Бухгалтерия", Salary = 30000 },
            new Employee() { Id = 2, Age = 25, FirstName = "Петр", LastName = "Петров", Patronymic = "Петрович", Department = "Отдел кадров", Salary = 35000 },
            new Employee() { Id = 3, Age = 30, FirstName = "Сидор", LastName = "Сидоров", Patronymic = "Сидорович", Department = "Юридический отдел", Salary = 40000 },
        };
    }
}
