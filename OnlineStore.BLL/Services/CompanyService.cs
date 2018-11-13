using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class CompanyService : ICompanyService
    {
        private IIdentityUnitOfWork DataBase { get; set; }

        public CompanyService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }

        public string AddCompany(CompanyDto companyDto)
        {
            var comp = DataBase.Companies.GetAll().FirstOrDefault(c => c.Name == companyDto.Name);
            if (comp != null)
            {
                return "Company already exists";
            }

            Company company = new Company
            {
                Name = companyDto.Name
            };

            DataBase.Companies.Create(company);
            DataBase.Save();

            return "OK";
        }

        public void EditCompany(CompanyDto companyDto)
        {
            var company = DataBase.Companies.Get(companyDto.Id);
            company.Name = companyDto.Name;

            DataBase.Companies.Update(company);
            DataBase.Save();
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Company, CompanyDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Company>, List<CompanyDto>>(DataBase.Companies.GetAll());
        }

        public CompanyDto GetCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                return new CompanyDto { Id = company.Id, Name = company.Name };
            }

            return null;
        }

        public void DeleteCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                var productDtos = GetCertainBrandProducts(company.Id);
                if (productDtos != null)
                {
                    DeleteProducts(productDtos);
                    DataBase.Companies.Delete(company.Id);
                    DataBase.Save();
                }
            }
        }

        private IEnumerable<ProductDto> GetCertainBrandProducts(int companyId)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Product, ProductDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Product>, List<ProductDto>>(DataBase.Products.GetAll()
                .Where(t => t.CompanyId == companyId));
        }

        private void DeleteProducts(IEnumerable<ProductDto> productDtos)
        {
            foreach (var productDto in productDtos)
            {
                if (productDto != null)
                {
                    DataBase.LineItems.Delete(productDto.Id);
                }
            }
        }

        public IEnumerable<CompanyDto> GetCertainCategoryCompanies(int categoryId)
        {
            var category = DataBase.Categories.Get(categoryId);
            if (category == null) return null;

            var companies = DataBase.Companies
                .Find(c => c.Products.Any(p => p.CategoryId == categoryId));
            if (companies == null) return null;

            var mapper = new MapperConfiguration(c => c.CreateMap<Company, CompanyDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Company>, List<CompanyDto>>(companies);


        }
    }
}
