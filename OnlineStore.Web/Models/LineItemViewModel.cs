using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore.Web.Models
{
    public class LineItemViewModel
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int PhoneId { get; set; }
        public PhoneViewModel PhoneViewModel { get; set; }

        public int OrderId { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
    }
}