﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Services.Services.InSQL
{
    public class SqlProductData : IProductData //Это UnitOfWork
    {
        private readonly WebStoreDB _db;

        public SqlProductData(WebStoreDB db)
        {
            _db = db;
        }

        public BrandDTO GetBrandById(int id) => _db.Brands
            .Include(s => s.Products)
            .FirstOrDefault(s=>s.Id==id)
            .ToDTO();
        public IEnumerable<BrandDTO> GetBrands() => _db.Brands.Include(b => b.Products).ToDTO();

        public ProductDTO GetProductById(int id) => _db.Products
            .Include(p => p.Section)
            .Include(p => p.Brand)
            .FirstOrDefault(p => p.Id == id).ToDTO();

        public PageProductsDTO GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products
                .Include(p => p.Section)
                .Include(p => p.Brand);

            if (Filter?.Ids?.Length > 0)
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

            var total_count = query.Count();

            //if (Filter?.PageSize > 0)
            //    query = query
            //        .Skip((Filter.Page - 1) * (int)Filter.PageSize)
            //        .Take((int)Filter.PageSize);

            if (Filter is { PageSize: > 0 and var page_size, Page: > 0 and var page_number})
                query = query
                    .Skip((page_number - 1) * page_size)
                    .Take(page_size);

            return new PageProductsDTO( query.AsEnumerable().ToDTO(),total_count);
        }

        public SectionDTO GetSectionById(int id) => _db.Sections
            .Include(s => s.Products)
            .FirstOrDefault(s => s.Id == id)
            .ToDTO();
        public IEnumerable<SectionDTO> GetSections() => _db.Sections.Include(s => s.Products).ToDTO();
    }
}
