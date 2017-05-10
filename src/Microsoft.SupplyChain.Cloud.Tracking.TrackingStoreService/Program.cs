using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration;
using Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Configuration;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService
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

                ServiceRuntime.RegisterServiceAsync("TrackingStoreServiceType",ServiceFactory.CreateService).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(TrackingStoreService).Name);

                // Prevents this host process from terminating so services keep running.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
