using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OnlineStore.BLL.DTO;
using OnlineStore.BLL.Interfaces;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Interfaces;

namespace OnlineStore.BLL.Services
{
    public class OrderService : IOrderService
    {
        private IIdentityUnitOfWork DataBase { get; set; }

        public OrderService(IIdentityUnitOfWork dataBase)
        {
            DataBase = dataBase;
        }

        public void AddPhone(PhoneDto phoneDto)
        {
            Company company = DataBase.Companies.Get(phoneDto.CompanyId);
            if (company != null)
            {
                Phone phone = new Phone
                {
                    Name = phoneDto.Name,
                    CompanyId = company.Id,
                    PhoneDescription = phoneDto.PhoneDescription,
                    Price = phoneDto.Price
                };
                DataBase.Phones.Create(phone);
                DataBase.SaveAsync();
            }
        }

        public void DeletePhone(int id)
        {
            var phone = DataBase.Phones.Get(id);
            if (phone != null)
            {
                DataBase.Phones.Delete(phone.Id);
                DataBase.SaveAsync();
            }
        }

        public void AddCompany(CompanyDto companyDto)
        {
            if (companyDto != null)
            {
                Company company = new Company
                {
                    Name = companyDto.Name
                };
                DataBase.Companies.Create(company);
                DataBase.SaveAsync();
            }

        }

        public void DeleteCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                DataBase.Companies.Delete(company.Id);
                DataBase.SaveAsync();
            }
        }

        public CompanyDto GetCompany(int? id)
        {
            if (id != null)
            {
                var company = DataBase.Companies.Get(id.Value);
                if (company != null)
                {
                    return new CompanyDto { Id = company.Id, Name = company.Name };
                }

                return null;
            }
            return null;
        }

        public IEnumerable<CompanyDto> GetCompanies()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Company, CompanyDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Company>, List<CompanyDto>>(DataBase.Companies.GetAll());
        }

        public PhoneDto GetPhone(int? id)
        {
            if (id != null)
            {
                var phone = DataBase.Phones.Get(id.Value);
                if (phone != null)
                {
                    return new PhoneDto { Name = phone.Name, CompanyId = phone.CompanyId, PhoneDescription = phone.PhoneDescription, Price = phone.Price, Id = phone.Id };
                }

                return null;
            }
            return null;
        }

        public IEnumerable<PhoneDto> GetPhones()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Phone, PhoneDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Phone>, List<PhoneDto>>(DataBase.Phones.GetAll());
        }
        public IEnumerable<PhoneDto> GetCertainBrandPhones(int? companyId)
        {
            if (companyId != null)
            {
                var company = DataBase.Companies.Get(companyId.Value);
                if (company != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<Phone, PhoneDto>()).CreateMapper();
                    return mapper.Map<IEnumerable<Phone>, List<PhoneDto>>(DataBase.Phones.GetAll().Where(t => t.Company == company));
                }
            }
            return null;
        }


        public void MakeOrder(OrderDto orderDto)
        {
            throw new NotImplementedException();
        }

        public void CanceleOrder(OrderDto orderDto)
        {
            throw new NotImplementedException();
        }




        public LineItemDto GeLineItemDto(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LineItemDto> GetLineItemDtos()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
