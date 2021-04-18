using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Infrastructure.Services.InSQL
{
    public class SqlProductData : IProductData //Это UnitOfWork
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public IEnumerable<Brand> GetBrands() => _db.Brands.Include(b => b.Products);

        public Product GetProductById(int id) => _db.Products
            .Include(p=>p.Section)
            .Include(p=>p.Brand)
            .FirstOrDefault(p => p.Id == id);

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Section)
                .Include(p => p.Brand);

            if (Filter?.Ids?.Length>0)
            {
                query = query.Where(product => Filter.Ids.Contains(product.Id));
            }
            else
            {
                if (Filter?.SectionId is { } section_id)
                    query = query.Where(product => product.SectionId == section_id);

                if (Filter?.BrandId is { } brandId)
                    query = query.Where(product => product.BrandId == brandId);
            }

            return query;
        }

        public IEnumerable<Section> GetSections() => _db.Sections.Include(s => s.Products);
    }
}
