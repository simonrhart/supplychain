using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                IContainerBuilder containerBuilder = new DefaultContainerBuilder();
                containerBuilder.Build();

                ServiceRuntime.RegisterServiceAsync("DiscoveryServiceType", ServiceFactory).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(DiscoveryService).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static StatelessService ServiceFactory(StatelessServiceContext context)
        {
            // pass in dependencies as there is no other way to do it with the SF c# sdk.
            var service = new DiscoveryService(context);
            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<IDiscoveryService>().Instance(service));
            return service;
        }
    }
}
