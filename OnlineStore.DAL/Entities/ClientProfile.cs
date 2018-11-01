using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.DAL.Entities
{
    public class ClientProfile
    {
        public ClientProfile()
        {
            Orders = new List<Order>();
        }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
