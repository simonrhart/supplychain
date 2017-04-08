using System.Web.Http;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration;
using Owin;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService
{
    public static class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            IContainerBuilder containerBuilder = new DefaultContainerBuilder();
            containerBuilder.Build();

            config.DependencyResolver = new CastleDependencyResolver(containerBuilder.Container);

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
