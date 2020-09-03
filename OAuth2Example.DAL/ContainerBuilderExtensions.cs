using Autofac;
using System.Data.Entity;

namespace OAuth2Example.DAL
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterDal(this ContainerBuilder builder)
        {
            builder.RegisterType<OAuth2ExampleDbContext>().As<DbContext>()
                .InstancePerRequest();

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>()
                .InstancePerRequest();
        }
    }
}
