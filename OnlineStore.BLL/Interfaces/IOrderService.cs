using System.Collections.Generic;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface IOrderService
    {
        void MakeOrder(OrderDto orderDto, IEnumerable<LineItemDto> lineItemDtos, string userId);
        IEnumerable<OrderDto> GetOrders();
        IEnumerable<OrderDto> GetOrdersForUser(string userId);
        OrderDto GetOrder(int id);
        void DeleteOrder(int id);
        void EditOrder(OrderDto orderDto);

        ProductDto GetProduct(int? id);
        ProductDto[] GetProducts(int[] ids);
        IEnumerable<ProductDto> GetProducts();
        IEnumerable<ProductDto> GetCertainBrandProducts(int? companyId, int categoryId);
        IEnumerable<ProductDto> GetCertainCategoryProducts(int categoryId);


        void AddProduct(ProductDto product);
        void DeleteProduct(int id);

        string AddCompany(CompanyDto company);
        void DeleteCompany(int id);
        CompanyDto GetCompany(int id);
        IEnumerable<CompanyDto> GetCompanies();
        IEnumerable<CompanyDto> GetCertainCategoryCompanies(int categoryId);

        string AddCategory(CategoryDto category);
        void DeleteCategory(int id);
        CategoryDto GetCategory(int id);
        IEnumerable<CategoryDto> GetCategories();


        IEnumerable<LineItemDto> GetLineItemDtos(int orderId);
        LineItemDto GetLineItemDto(int id);
        void DeleteLineItem(int id);

        UserDto GetUserData(string id);


        void Dispose();
    }
}
