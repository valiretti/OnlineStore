using Microsoft.AspNet.Identity;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Identity
{
   public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> store) : base(store)
        {
        }
    }
}
