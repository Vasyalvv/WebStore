using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required, MaxLength(256)]
        [Display(Name = "Имя пользователя")]
        [Remote("IsNameFree","Account")]
        public string UserName { get; init; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; init; }

        [Required]
        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirm { get; init; }
    }
}
