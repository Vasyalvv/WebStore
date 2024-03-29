﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private const string __CatalogPageSize = "CatalogPageSize";
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Index(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            var page_size = PageSize
                ?? (int.TryParse(_Configuration[__CatalogPageSize], out var value) ? value : null);

            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Page = Page,
                PageSize = page_size
            };
            var (products, total_count) = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products
            .OrderBy(p => p.Order).FromDTO()
            //.Select(p => new ProductViewModel
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    ImageUrl = p.ImageUrl,
            //    Price = p.Price
            //})
            .ToView(),
                PageViewModel = new PageViewModel
                {
                    Page = Page,
                    PageSize = page_size ?? 0,
                    TotalItems = total_count
                }
            });
        }

        public IActionResult Details(int id)
        {
            var product = _ProductData.GetProductById(id);
            if (product is null) return NotFound();

            return View(product.FromDTO().ToView());
        }

        #region WebApi

        public IActionResult GetFeaturesItems(int? BrandId, int? SectionId, int Page = 1, int? PageSize = null)
        {
            return PartialView("Partial/_FeaturesItems", GetProducts(BrandId,SectionId,Page,PageSize));
        }

        private IEnumerable<ProductViewModel> GetProducts(int? BrandId, int? SectionId, int Page, int? PageSize) =>
            _ProductData.GetProducts(new ProductFilter 
            { 
                BrandId=BrandId,
                SectionId=SectionId,
                Page=Page,
                PageSize=PageSize ?? (int.TryParse(_Configuration[__CatalogPageSize], out var value) ? value : null)
            })
                .Products.OrderBy(p => p.Order)
                .FromDTO()
                .ToView();

        #endregion
    }
}
