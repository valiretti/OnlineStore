using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlineStore.DAL.Entities
{
   public class ApplicationUser : IdentityUser
    {
        public virtual ClientProfile ClientProfile { get; set; }
    }
}
