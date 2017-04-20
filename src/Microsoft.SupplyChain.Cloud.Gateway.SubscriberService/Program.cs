using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Configuration;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService
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
                
                ServiceRuntime.RegisterServiceAsync("SubscriberServiceType", ServiceFactory).GetAwaiter().GetResult();               
                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(SubscriberService).Name);
                                            

                // Prevents this host process from terminating so services keep running.
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
          
            var service = new SubscriberService(context);
            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<ISubscriberService>().Instance(service));           
            return service;
        }
    }
}
