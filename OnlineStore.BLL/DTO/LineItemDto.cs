using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.DTO
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public int PhoneId { get; set; }
        public int OrderId { get; set; }
    }
}
