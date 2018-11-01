using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using OnlineStore.BLL.Infrastructure;
using OnlineStore.BLL.Interfaces;
using OnlineStore.BLL.Services;

namespace OnlineStore.Web.Util
{
    public class Autofac
    {
        public static void ConfigureUserServise()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<BusinessModule>();

            builder.RegisterType<UserService>()
                .As<IUserService>()
               .InstancePerRequest();

            builder.RegisterType<OrderService>()
                .As<IOrderService>()
                .InstancePerRequest();

            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
       

    }
}