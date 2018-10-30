using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.BLL.DTO
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public string ImagePath { get; set; }
        public string ProductDescription { get; set; }
        public decimal Price { get; set; }

    }
}
