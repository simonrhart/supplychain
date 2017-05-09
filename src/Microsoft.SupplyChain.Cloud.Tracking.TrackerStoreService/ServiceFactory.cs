using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService
{
    public static class ServiceFactory
    {
        public static StatelessService CreateService(StatelessServiceContext context)
        {
            // pass in dependencies as there is no other way to do it with the SF c# sdk.
            var configurationPackage = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var iotHubSection = configurationPackage.Settings.Sections["IoTHub"].Parameters;

            // create the registry manager here in the factory so we can more easilly test the repository.
            string iotHubConnectionString = iotHubSection["IoTHubConnectionString"].Value;
            ITrackerStoreRepository trackerStoreRepository = new TrackerStoreRepository(RegistryManager.CreateFromConnectionString(iotHubConnectionString));
            var service = new DeviceStoreService(context, deviceStoreRepository);
            return service;
        }
    }
}
