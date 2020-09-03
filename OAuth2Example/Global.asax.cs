using OAuth2Example.App_Start;
using System.Web.Http;

namespace OAuth2Example
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AutoFacConfig.Configure();
            AutoMapperConfig.Configure();
        }
    }
}
