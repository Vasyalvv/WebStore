using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModels
{
    public class EmployeeViewModel //:IValidatableObject
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = "Фамилия является обязательной")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|[A-Z][a-z]+", ErrorMessage = "Неправильный формат: все символы должны быть либо латиницей, либо кириллицей. Первая буква - заглавная")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = "Имя является обязательным")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Длина должна быть от 2 до 200 символов")]
        [RegularExpression(@"([А-ЯЁ][а-яё]+)|[A-Z][a-z]+", ErrorMessage = "Неправильный формат: все символы должны быть либо латиницей, либо кириллицей. Первая буква - заглавная")]

        public string Name { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(200, ErrorMessage = "Длина должна быть до 200 символов")]

        public string Patronymic { get; set; }

        [Display(Name = "Возраст")]
        [Range(18, 80, ErrorMessage = "Возраст должен быть от 18 до 80 лет")]
        public int Age { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    yield return ValidationResult.Success;
        //    yield return new ValidationResult("Ошибка валидации", new[]{nameof(LastName), nameof(Age)});
        //}
    }
}
