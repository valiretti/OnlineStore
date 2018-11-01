using Microsoft.AspNet.Identity;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}
