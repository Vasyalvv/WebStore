using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Interfaces;

namespace WebStore.Domain.Entities
{
    //[Table("Brands")] //Демонстрация атрибутов 
    public class Brand : NamedEntity, IOrderedEntity
    {
        //[Column("BrandOrder",TypeName ="int")] //Демонстрация атрибутов 
        public int Order { get ; set ; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
