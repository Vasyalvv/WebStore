﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Services.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;
        private readonly UserManager<User> _UserManager;
        private readonly RoleManager<Role> _RoleManager;

        public WebStoreDbInitializer(
            WebStoreDB db,
            ILogger<WebStoreDbInitializer> Logger,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager)
        {
            _db = db;
            _Logger = Logger;
            _UserManager = UserManager;
            _RoleManager = RoleManager;
        }

        public void Initialize()
        {
            _Logger.LogInformation("Инициализация БД...");
            //_db.Database.EnsureDeleted(); //Удаляет БД, если она существует
            //_db.Database.EnsureCreated(); //Создает БД, если ее нет

            if (_db.Database.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("Выполнение миграции БД...");
                _db.Database.Migrate(); //Применяет миграции
                _Logger.LogInformation("Выполнение миграции БД выполнена успешно");
            }

            try
            {
                InitializeProducts();
            }
            catch (Exception)
            {
                _Logger.LogError("Ошибка при инициализации товаров в БД");
                throw;
            }

            try
            {
                InitializeIdentityAsync().GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                _Logger.LogError("Ошибка при инициализации данных системы Identity");
                throw;
            }

            _Logger.LogInformation("Инициализация БД выполнена успешно");
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация товаров не требуется");
                return;
            }

            var products_sections = TestData.Sections.Join(
                TestData.Products,
                section => section.Id,
                product => product.SectionId,
                (section, product) => (section, product));

            foreach (var (section, product) in products_sections)
            {
                section.Products.Add(product);
            }

            var products_brands = TestData.Brands.Join(
                TestData.Products,
                brand => brand.Id,
                product => product.BrandId,
                (brand, product) => (brand, product));

            foreach (var (brand, product) in products_brands)
            {
                brand.Products.Add(product);
            }

            var section_section = TestData.Sections.Join(
                TestData.Sections,
                parent => parent.Id,
                child => child.ParentId,
                (parent, child) => (parent, child));

            foreach (var (parent, child) in section_section)
            {
                child.Parent = parent;
            }

            foreach (var product in TestData.Products)
            {
                product.Id = 0;
                product.BrandId = null;
                product.SectionId = 0;
            }

            foreach (var brand in TestData.Brands)
            {
                brand.Id = 0;
            }

            foreach (var section in TestData.Sections)
            {
                section.Id = 0;
                section.ParentId = null;
            }

            _Logger.LogInformation("Инициализация таблиц БД...");
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);
                _db.Brands.AddRange(TestData.Brands);
                _db.Products.AddRange(TestData.Products);

                _db.SaveChanges();
                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Инициализация таблиц БД завершена");

            _Logger.LogInformation("Инициализация товаров завершена");
        }

        private async Task InitializeIdentityAsync()
        {
            _Logger.LogInformation("Инициализация БД системы Identity");

            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                {
                    _Logger.LogInformation("Роль {0} отсутствует. Процесс создания...", RoleName);

                    await _RoleManager.CreateAsync(new Role { Name = RoleName });

                    _Logger.LogInformation("Роль {0} создана успешно", RoleName);
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);

            if (await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                _Logger.LogInformation("Отсутствует учетная запись {0}. Процесс создания...", User.Administrator);

                var admin = new User { UserName = User.Administrator };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (creation_result.Succeeded)
                {
                    _Logger.LogInformation("Учетная запись {0} создана успешно", User.Administrator);

                    await _UserManager.AddToRoleAsync(admin, Role.Administrators);

                    _Logger.LogInformation("Учетная запись {0} наделена ролью администратора", User.Administrator);
                }
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Code);

                    _Logger.LogInformation($"Ошибка при создании учетной записи администратора: {string.Join(",", errors)}");
                    throw new InvalidOperationException($"Ошибка при создании учетной записи администратора: {string.Join(",", errors)}");
                }
            }
            _Logger.LogInformation("Инициализация БД системы Identity выполнена");

        }
    }
}
