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
        void MakeOrder(OrderDto orderDto);
        void CanceleOrder(OrderDto orderDto);
      
        PhoneDto GetPhone(int? id);
        IEnumerable<PhoneDto> GetPhones();
        IEnumerable<PhoneDto> GetCertainBrandPhones(int? companyId);

        void AddPhone(PhoneDto phone);
        void DeletePhone(int id);

        void AddCompany(CompanyDto company);
        void DeleteCompany(int id);

        CompanyDto GetCompany(int? id);
        IEnumerable<CompanyDto> GetCompanies();

        LineItemDto GeLineItemDto(int? id);
        IEnumerable<LineItemDto> GetLineItemDtos();



        void Dispose();
    }
}
