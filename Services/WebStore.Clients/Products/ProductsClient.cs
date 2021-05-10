using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient , IProductData
    {
        public ProductsClient(IConfiguration Configuration) : base(Configuration, WebAPI.Products)
        {
        }

        public BrandDTO GetBrandById(int id) => Get<BrandDTO>($"{Address}/brands/{id}");
        public IEnumerable<BrandDTO> GetBrands() => base.Get<IEnumerable<BrandDTO>>($"{Address}/brands");
        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{Address}/{id}");
        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) => 
            Post(Address,Filter?? new ProductFilter())
            .Content
            .ReadAsAsync<IEnumerable<ProductDTO>>()
            .Result;
        public SectionDTO GetSectionById(int id) => Get<SectionDTO>($"{Address}/sections/{id}");
        public IEnumerable<SectionDTO> GetSections() => base.Get<IEnumerable<SectionDTO>>($"{Address}/sections");
    }
}
