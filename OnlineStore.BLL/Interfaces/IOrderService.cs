using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface IOrderService
    {
        void MakeOrder(OrderDto orderDto, IEnumerable<LineItemDto> lineItemDtos);
        IEnumerable<OrderDto> GetOrders();
        OrderDto GetOrder(int id);
        void DeleteOrder(int id);
        void CanceleOrder(int orderId);
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
   
   


        void Dispose();
    }
}
