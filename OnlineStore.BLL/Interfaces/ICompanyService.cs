using System.Collections.Generic;
using OnlineStore.BLL.DTO;

namespace OnlineStore.BLL.Interfaces
{
    public interface ICompanyService
    {
        string AddCompany(CompanyDto company);
        void EditCompany(CompanyDto company);
        void DeleteCompany(int id);
        CompanyDto GetCompany(int id);
        IEnumerable<CompanyDto> GetCompanies();
        IEnumerable<CompanyDto> GetCertainCategoryCompanies(int categoryId);
    }
}
