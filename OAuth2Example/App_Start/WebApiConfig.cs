using OAuth2Example.Utils;
using System.Web.Http;
using System.Web.Http.Cors;

namespace OAuth2Example
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Origins: The allowed domains which can make requests to this API, * for any
            // Headers: Allowed headers that the cliet can set, * for any
            // Methods: Allowed HTTP methods that the client can use, * for any
            //
            // Example: 
            //      new EnableCorsAttribute("http://example.com", headers: "Accept,Content-Type,Origin", methods: "*");
            //
            // More information:
            //      https://developer.mozilla.org/en-US/docs/Web/HTTP/Access_control_CORS
            config.EnableCors(new EnableCorsAttribute(origins: "*", headers: "*", methods: "GET,POST,PATCH,DELETE"));

            config.MapHttpAttributeRoutes(new InheritedDirectRouteProvider());

            config.Routes.MapHttpRoute(
                name: "LearnToLearnApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
