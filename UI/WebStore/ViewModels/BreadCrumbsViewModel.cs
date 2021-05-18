using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.ViewModels
{
    public class BreadCrumbsViewModel
    {
        public Section Section { get; set; }

        public Brand Brand { get; set; }

        public string Product { get; set; }
    }
}
