using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Entities
{
    public class LineItem
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int? PhoneId { get; set; }
        public Phone Phone { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }
        
    }
}
