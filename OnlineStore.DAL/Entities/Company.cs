using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DAL.Entities
{
   public class Company
    {
        public Company()
        {
            Phones = new List<Phone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }
}
