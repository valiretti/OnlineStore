using System;
using System.Collections.Generic;
using System.Linq;
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
                    Price = phoneDto.Price,
                    ImagePath = phoneDto.ImagePath

                };
                DataBase.Phones.Create(phone);
                DataBase.Save();
            }
        }

        public void DeletePhone(int id)
        {
            var phone = DataBase.Phones.Get(id);
            if (phone != null)
            {
                DataBase.Phones.Delete(phone.Id);
                DataBase.Save();
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
                DataBase.Save();
            }

        }

        public void DeleteCompany(int id)
        {
            var company = DataBase.Companies.Get(id);
            if (company != null)
            {
                var phoneDtos = GetCertainBrandPhones(company.Id);
                if (phoneDtos != null)
                {
                    DeletePhones(phoneDtos);
                    DataBase.Companies.Delete(company.Id);
                    DataBase.Save();
                }
            }
        }

        private void DeletePhones(IEnumerable<PhoneDto> phoneDtos)
        {
            foreach (var phoneDto in phoneDtos)
            {
                if (phoneDto != null)
                {
                    DataBase.LineItems.Delete(phoneDto.Id);
                }
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
        public PhoneDto[] GetPhones(int[] ids)
        {
            var phones = DataBase.Phones.Find(t => ids.Contains(t.Id)).ToList();
            var mapper = new MapperConfiguration(c => c.CreateMap<Phone, PhoneDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Phone>, PhoneDto[]>(phones);
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

        public UserDto GetUserData(string id)
        {
            var user = DataBase.ClientManager.GetClientProfile(id);
            if (user != null)
            {
                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                };
                return userDto;
            }

            return null;
        }

        public void MakeOrder(OrderDto orderDto, IEnumerable<LineItemDto> lineItemDtos, string userId)
        {
            if (orderDto != null && lineItemDtos != null)
            {
                Order order = new Order
                {
                    Address = orderDto.Address,
                    Date = DateTime.Now,
                    Email = orderDto.Email,
                    PhoneNumber = orderDto.PhoneNumber,
                    State = State.Оrdered,
                    Name = orderDto.Name,
                    ClientProfileId = userId
                };
                DataBase.Orders.Create(order);

                foreach (var lineItemDto in lineItemDtos)
                {
                    LineItem lineItem = new LineItem()
                    {
                        Count = lineItemDto.Count,
                        PhoneId = lineItemDto.PhoneId,
                        Order = order
                    };
                    DataBase.LineItems.Create(lineItem);
                }

                DataBase.Save();
            }
        }

        public IEnumerable<OrderDto> GetOrders()
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDto>>(DataBase.Orders.GetAll());
        }

        public IEnumerable<OrderDto> GetOrdersForUser(string userId)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
            return mapper.Map<IEnumerable<Order>, List<OrderDto>>(DataBase.Orders.GetAll().Where(t => t.ClientProfileId == userId));
        }

        public OrderDto GetOrder(int id)
        {

            var order = DataBase.Orders.Get(id);
            if (order != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<Order, OrderDto>()).CreateMapper();
                return mapper.Map<Order, OrderDto>(order);
            }
            return null;
        }

        public void EditOrder(OrderDto orderDto)
        {
            var order = new Order()
            {
                Id = orderDto.Id,
                Name = orderDto.Name,
                Address = orderDto.Address,
                Date = DateTime.Now,
                Email = orderDto.Email,
                PhoneNumber = orderDto.PhoneNumber,
                ClientProfileId = orderDto.ClientProfileId,
                State = orderDto.State == "Canceled" ? State.Canceled : orderDto.State == "Сonfirmed" ? State.Сonfirmed : State.Оrdered
            };
            DataBase.Orders.Update(order);
            DataBase.Save();
        }

        public void DeleteOrder(int id)
        {
            var order = DataBase.Orders.Get(id);
            if (order != null)
            {
                IEnumerable<LineItemDto> lineItemDtos = GetLineItemDtos(order.Id);
                if (lineItemDtos != null)
                {
                    DeleteLineItems(lineItemDtos);
                    DataBase.Orders.Delete(order.Id);
                    DataBase.Save();
                }
            }
        }

        private void DeleteLineItems(IEnumerable<LineItemDto> lineItemDtos)
        {
            foreach (var lineItemDto in lineItemDtos)
            {
                if (lineItemDto != null)
                {
                    DataBase.LineItems.Delete(lineItemDto.Id);
                }
            }
        }


        public IEnumerable<LineItemDto> GetLineItemDtos(int orderId)
        {
            var order = DataBase.Orders.Get(orderId);
            if (order != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItem, LineItemDto>()
                    .ForMember(x => x.OrderDto, s => s.MapFrom(t => t.Order)).
                    ForMember(x => x.PhoneDto, s => s.MapFrom(t => t.Phone))).CreateMapper();

                return mapper.Map<IEnumerable<LineItem>, List<LineItemDto>>(DataBase.LineItems.GetAll().Where(t => t.OrderId == order.Id));
            }
            return null;
        }

        public LineItemDto GetLineItemDto(int id)
        {
            var lineItem = DataBase.LineItems.Get(id);
            if (lineItem != null)
            {
                var mapper = new MapperConfiguration(c => c.CreateMap<LineItem, LineItemDto>()
                    .ForMember(x => x.OrderDto, s => s.MapFrom(t => t.Order)).
                    ForMember(x => x.PhoneDto, s => s.MapFrom(t => t.Phone))).CreateMapper();

                return mapper.Map<LineItem, LineItemDto>(DataBase.LineItems.Get(id));
            }
            return null;
        }

        public void DeleteLineItem(int id)
        {
            var lineItem = DataBase.LineItems.Get(id);
            if (lineItem != null)
            {
                DataBase.LineItems.Delete(lineItem.Id);
                DataBase.Save();
            }
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
