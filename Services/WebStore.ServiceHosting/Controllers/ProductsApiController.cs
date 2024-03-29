﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    [Route(WebAPI.Products)]
    [ApiController]
    public class ProductsApiController : ControllerBase,IProductData
    {
        private readonly IProductData _ProductData;

        public ProductsApiController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        [HttpGet("brands/{id:int}")]
        public BrandDTO GetBrandById(int id) => _ProductData.GetBrandById(id);
        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => _ProductData.GetBrands();

        [HttpGet("{id:int}")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);

        [HttpPost]
        public PageProductsDTO GetProducts(ProductFilter Filter = null) => _ProductData.GetProducts(Filter);
        [HttpGet("sections/{id:int}")]
        public SectionDTO GetSectionById(int id) => _ProductData.GetSectionById(id);
        [HttpGet("sections")]
        public IEnumerable<SectionDTO> GetSections() => _ProductData.GetSections();
    }
}
