using System;

namespace OnlineStore.BLL.DTO
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string State { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Date { get; set; }
        public string ClientProfileId { get; set; }

    }
}
