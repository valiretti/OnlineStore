using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using OnlineStore.DAL.Entities;

namespace OnlineStore.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store, IIdentityMessageService emailService) : base(store)
        {
            this.EmailService = emailService;

            var provider = new DpapiDataProtectionProvider("OnlineStore");
            this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("PasswordReset"));
        }
    }
}
