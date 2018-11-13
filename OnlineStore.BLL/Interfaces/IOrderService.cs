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

        

        

        


        IEnumerable<LineItemDto> GetLineItemDtos(int orderId);
        LineItemDto GetLineItemDto(int id);
        void DeleteLineItem(int id);

        UserDto GetUserData(string id);


        void Dispose();
    }
}
