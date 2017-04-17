using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService.Repositories;

namespace Microsoft.SupplyChain.Cloud.Administration.DeviceStoreService
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
                ServiceRuntime.RegisterServiceAsync("DeviceStoreServiceType", ServiceFactory).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(DeviceStoreService).Name);

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
            var configurationPackage = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var iotHubSection = configurationPackage.Settings.Sections["IoTHub"].Parameters;
         
            IDeviceStoreRepository deviceStoreRepository = new DeviceStoreRepository(iotHubSection["IoTHubConnectionString"].Value);
            var service = new DeviceStoreService(context, deviceStoreRepository);
            return service;
        }
    }
}
