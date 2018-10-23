using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Identity;

namespace OnlineStore.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }
}
