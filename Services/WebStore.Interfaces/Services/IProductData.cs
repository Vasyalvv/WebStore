using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain;
using WebStore.Domain.DTO;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        SectionDTO GetSectionById(int id);

        IEnumerable<SectionDTO> GetSections();

        BrandDTO GetBrandById(int id);

        IEnumerable<BrandDTO> GetBrands();

        IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null);

        ProductDTO GetProductById(int id);
    }
}
