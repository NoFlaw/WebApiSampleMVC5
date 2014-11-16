using System.Web.Http;
using Microsoft.Owin.Security.OAuth;

namespace WebApiSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API Attribute Routing (defining specific routes via Attribute ex: [Route()])
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                //constraints: new { id = @"^\d+$" } // Only integers 
            );
        }
    }
}
