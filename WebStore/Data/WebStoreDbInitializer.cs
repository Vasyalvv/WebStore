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

            var products_sections = TestData.Sections.Join(
                TestData.Products,
                section => section.Id,
                product => product.SectionId,
                (section, product) => (section, product));

            foreach (var (section,product) in products_sections)
            {
                section.Products.Add(product);
            }

            var products_brands = TestData.Brands.Join(
                TestData.Products,
                brand=>brand.Id,
                product=>product.BrandId,
                (brand,product)=>(brand,product));

            foreach (var (brand,product) in products_brands)
            {
                brand.Products.Add(product);
            }

            var section_section = TestData.Sections.Join(
                TestData.Sections,
                parent=>parent.Id,
                child=>child.ParentId,
                (parent,child)=>(parent,child));

            foreach (var (parent,child) in section_section)
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
    }
}
