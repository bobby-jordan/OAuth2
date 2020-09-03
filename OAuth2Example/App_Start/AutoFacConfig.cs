using Autofac;
using Autofac.Integration.WebApi;
using OAuth2Example.Auth;
using OAuth2Example.DAL;
using OAuth2Example.DataServices;
using System.Reflection;
using System.Web.Http;

namespace OAuth2Example.App_Start
{
    public static class AutoFacConfig
    {
        public static void Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            HttpConfiguration config = GlobalConfiguration.Configuration;

            builder.RegisterDal();
            builder.RegisterDataServices();
            builder.RegisterAuth();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            IContainer container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}