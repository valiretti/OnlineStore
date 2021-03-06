﻿namespace OnlineStore.BLL.DTO
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int ProductId { get; set; }
        public ProductDto ProductDto { get; set; }

        public int OrderId { get; set; }
        public OrderDto OrderDto { get; set; }
    }
}
