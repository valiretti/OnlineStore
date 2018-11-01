using System.Collections.Generic;

namespace OnlineStore.DAL.Entities
{
   public class Company
    {
        public Company()
        {
            Products = new List<Product>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
