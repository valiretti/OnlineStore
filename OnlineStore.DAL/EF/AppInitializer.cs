using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineStore.DAL.Entities;
using OnlineStore.DAL.Identity;

namespace OnlineStore.DAL.EF
{
    public class AppInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext context)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var role1 = new IdentityRole { Name = "Admin" };
            var role2 = new IdentityRole { Name = "Manager" };
            var role3 = new IdentityRole { Name = "User" };

            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);

            var admin = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com" };
            string password = "administrator";
            var result = userManager.Create(admin, password);

            if (result.Succeeded)
            {
                userManager.AddToRole(admin.Id, role1.Name);
                var clientProfile = new ClientProfile()
                {
                    Id = admin.Id,
                    Address = "ул. Спортивная, д.30, кв.75",
                    Name = "Admin",
                    Email = "admin@gmail.com",
                    PhoneNumber = "1234567"
                };
                context.ClientProfiles.Add(clientProfile);
                context.SaveChanges();
            }

            base.Seed(context);
        }
    }
}
