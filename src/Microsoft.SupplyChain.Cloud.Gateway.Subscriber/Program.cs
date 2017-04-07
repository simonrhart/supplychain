using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Configuration;
using Microsoft.SupplyChain.Framework;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber
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
                
                ServiceRuntime.RegisterServiceAsync("SubscriberType", ServiceFactory).GetAwaiter().GetResult();               
                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Subscriber).Name);
                                            

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
            var service = new Subscriber(context, 
                ServiceLocator.Current.GetInstance<ICommand<IoTHubSubscriberContext>>(),
                ServiceLocator.Current.GetInstance<ICommand<BlockchainContractBootstrapperContext>>());

            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<ISubscriber>().Instance(service));           
            return service;
        }
    }
}
