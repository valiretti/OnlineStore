using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using OnlineStore.DAL.Interfaces;
using OnlineStore.DAL.Repositories;

namespace OnlineStore.BLL.Infrastructure
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IdentityUnitOfWork>()
                .As<IIdentityUnitOfWork>()
                .WithParameter("connectionString", "DefaultConnection")
                .InstancePerRequest();
        }
    }
}
