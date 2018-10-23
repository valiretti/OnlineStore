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

        PhoneDto GetPhone(int? id);
        PhoneDto[] GetPhones(int[] ids);
        IEnumerable<PhoneDto> GetPhones();
        IEnumerable<PhoneDto> GetCertainBrandPhones(int? companyId);

        void AddPhone(PhoneDto phone);
        void DeletePhone(int id);

        void AddCompany(CompanyDto company);
        void DeleteCompany(int id);

        CompanyDto GetCompany(int? id);
        IEnumerable<CompanyDto> GetCompanies();

        IEnumerable<LineItemDto> GetLineItemDtos(int orderId);
        LineItemDto GetLineItemDto(int id);
        void DeleteLineItem(int id);

        UserDto GetUserData(string id);


        void Dispose();
    }
}
