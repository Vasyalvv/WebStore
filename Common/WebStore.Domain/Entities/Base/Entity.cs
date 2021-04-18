using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Interfaces;

namespace WebStore.Domain.Entities.Base
{
    public abstract class Entity : IEntity
    {
        [Display(Name ="ID")]
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)] //Здесь это не обязательно, просто пример демонстрации атрибутов
        public int Id { get ; set ; }
    }
}
