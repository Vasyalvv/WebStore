﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Patronymic { get; set; }

        public int Age { get; set; }

        public string Department { get; set; }

        public int Salary { get; set; }
        public override string ToString() => $"{LastName} {FirstName} {Patronymic} {Age} лет";
    }
}
