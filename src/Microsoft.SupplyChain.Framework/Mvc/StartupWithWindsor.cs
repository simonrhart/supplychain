using System.Web.Http;
using Castle.Windsor;
using Owin;

namespace Microsoft.SupplyChain.Framework.Mvc
{
    public static class StartupWithWindsor
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration
            {
                DependencyResolver =
                    new CastleDependencyResolver(ServiceLocator.Current.GetInstance<IWindsorContainer>())
            };

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            appBuilder.UseWebApi(config);
        }
    }
}
