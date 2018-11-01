using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationContext()
        {
            Database.SetInitializer<ApplicationContext>(new AppInitializer());
        }

        public ApplicationContext(string conectionString) : base(conectionString) { }

        public DbSet<ClientProfile> ClientProfiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
    }
}
