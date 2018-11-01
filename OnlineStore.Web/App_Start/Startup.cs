using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using OnlineStore.BLL.Interfaces;
using OnlineStore.Web;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace OnlineStore.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<IUserService>());

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

       
    }
}