using System.Collections.Generic;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
   public interface IProductService
    {
        ProductDto GetProduct(int? id);
        ProductDto[] GetProducts(int[] ids);
        IEnumerable<ProductDto> GetProducts();
        IEnumerable<ProductDto> GetCertainBrandProducts(int? companyId, int categoryId);
        IEnumerable<ProductDto> GetCertainCategoryProducts(int categoryId);


        void AddProduct(ProductDto product);
        void DeleteProduct(int id);
    }
}
