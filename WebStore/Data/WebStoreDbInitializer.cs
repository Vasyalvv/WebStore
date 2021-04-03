using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db,ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _Logger = Logger;
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

            _Logger.LogInformation("Инициализация БД выполнена успешно");
        }

        private void InitializeProducts()
        {
            if (_db.Products.Any())
            {
                _Logger.LogInformation("Инициализация товаров не требуется");
                return;
            }


            _Logger.LogInformation("Инициализация секций...");
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Инициализация секций завершена");

            _Logger.LogInformation("Инициализация брендов...");
            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Инициализация брендов завершена");

            _Logger.LogInformation("Инициализация товаров...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");

                _db.SaveChanges();

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Инициализация товаров завершена");
        }
    }
}
