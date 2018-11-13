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
                        ProductId = lineItemDto.ProductId,
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
                    ForMember(x => x.ProductDto, s => s.MapFrom(t => t.Product))).CreateMapper();

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
                    ForMember(x => x.ProductDto, s => s.MapFrom(t => t.Product))).CreateMapper();

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
                    Address = user.Address
                };
                return userDto;
            }

            return null;
        }

        public void Dispose()
        {
            DataBase.Dispose();
        }
    }
}
