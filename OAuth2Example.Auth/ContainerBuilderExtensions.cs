using Autofac;
using Autofac.Integration.WebApi;
using OAuth2Example.Auth.Filters;
using System.Web.Http;

namespace OAuth2Example.Auth
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterAuth(this ContainerBuilder builder)
        {
            builder.RegisterType<OAuth2AuthService>().As<IHttpAuthService>()
                .InstancePerRequest();

            builder.RegisterType<OAuth2AuthenticationFilter>().AsWebApiActionFilterFor<ApiController>()
                .InstancePerRequest();
        }
    }
}
