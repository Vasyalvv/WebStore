using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int SectionId { get; set; }

        [Display(Name = "Секция")]
        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }

        public int? BrandId { get; set; }

        [Display(Name = "Бренд")]
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }

        [Display(Name = "Картинка")]
        public string ImageUrl { get; set; }

        [Display(Name = "Цена")]
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
