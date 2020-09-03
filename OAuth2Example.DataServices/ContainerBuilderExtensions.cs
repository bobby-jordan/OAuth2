using Autofac;
using OAuth2Example.DAL.Models;

namespace OAuth2Example.DataServices
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterDataServices(this ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IService<User>>()
                .InstancePerRequest();
            builder.RegisterType<TokenService>().As<IService<AuthToken>>()
                .InstancePerRequest();
        }
    }
}
