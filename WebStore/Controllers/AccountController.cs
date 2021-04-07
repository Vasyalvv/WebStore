using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _UserManager;
        private readonly SignInManager<User> _SignInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> UserManager,
            SignInManager<User> SignInManager,
            ILogger<AccountController> Logger)
        {
            _UserManager = UserManager;
            _SignInManager = SignInManager;
            _Logger = Logger;
        }

        #region Register
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            _Logger.LogInformation("Регистрация пользователя {0}", user.UserName);

            var registration_result = await _UserManager.CreateAsync(user, Model.Password);

            if(registration_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} успешно зарегистрирован", user.UserName);

                await _UserManager.AddToRoleAsync(user, Role.Users);

                _Logger.LogInformation("Пользователь {0} наделен ролью {1}", user.UserName, Role.Users);

                await _SignInManager.SignInAsync(user, false);

                _Logger.LogInformation("Пользователь {0} успешно вошел в систему сразу после регистрации", user.UserName);

                return RedirectToAction("Index", "Home");
            }

            _Logger.LogWarning("Ошибка при регистрации пользователя {0}, ошибка: {1}",
                user.UserName,
                string.Join(",", registration_result.Errors.Select(e => e.Description)));

            foreach (var error in registration_result.Errors)            
                ModelState.AddModelError("",error.Description);            

            return View(Model);
        }
        #endregion


        #region Login
        public IActionResult Login(string ReturnUrl) => View(new LoginViewModel { ReturnUrl = ReturnUrl });

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _SignInManager.PasswordSignInAsync(
                Model.UserName,
                Model.Password,
                Model.RememberMe,
#if DEBUG
                false
#else
                true
#endif
                );

            if (login_result.Succeeded)
            {
                _Logger.LogInformation("Пользователь {0} вошел систему", Model.UserName);

                //if (Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //return RedirectToAction("Index", "Home");
                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            _Logger.LogWarning("Ошибка при вводе имени пользователя {0} или пароля", Model.UserName);

            ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            return View(Model);
        }
        #endregion

        public async Task<IActionResult> Logout()
        {
            var user_name = User.Identity.Name; 
            await _SignInManager.SignOutAsync();

            _Logger.LogInformation("Пользователь {0} вышел из системы", user_name);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied(string ReturnUrl)
        {
            var user_name = User.Identity.Name;
            _Logger.LogInformation("Ограничен доступ пользователю {0}", user_name);

            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }
    }
}
