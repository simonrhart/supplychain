using System.Web.Http;
using Castle.Windsor;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration;
using Microsoft.SupplyChain.Framework;
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
            
            config.DependencyResolver = new CastleDependencyResolver(ServiceLocator.Current.GetInstance<IWindsorContainer>());

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
