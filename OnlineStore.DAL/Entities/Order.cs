using System;
using System.Collections.Generic;

namespace OnlineStore.DAL.Entities
{
    public class Order
    {
        public Order()
        {
            LineItems = new List<LineItem>();
        }

        public int Id { get; set; }
        public State State { get; set; }

        public ICollection<LineItem> LineItems { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public DateTime Date { get; set; }

        public string ClientProfileId { get; set; }
        public ClientProfile ClientProfile { get; set; }
    }

    public enum State
    {
        Оrdered=0,
        Сonfirmed=1,
        Canceled=2

    }
}
