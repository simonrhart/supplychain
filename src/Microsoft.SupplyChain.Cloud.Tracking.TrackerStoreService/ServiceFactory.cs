using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService.Repositories;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService
{
    public static class ServiceFactory
    {
        public static StatelessService CreateService(StatelessServiceContext context)
        {
            // pass in dependencies as there is no other way to do it with the SF c# sdk.
            var configurationPackage = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var documentDbSection = configurationPackage.Settings.Sections["DocumentDB"].Parameters;
            var uri = documentDbSection["DocumentDBEndpointUri"].Value;
            if (string.IsNullOrEmpty(uri))
                throw new ArgumentNullException("DocumentDBEndpointUri", "DocumentDBEndpointUri is not set in Service Fabric configuration package.");

            var documentDbPrimaryKey = documentDbSection["DocumentDBPrimaryKey"].Value;
            if (string.IsNullOrEmpty(documentDbPrimaryKey))
                throw new ArgumentNullException("DocumentDBPrimaryKey", "DocumentDBPrimaryKey is not set in Service Fabric configuration package.");

            var documentClient = new DocumentClient(new Uri(uri), documentDbPrimaryKey);
            
            ITrackerStoreRepository trackerStoreRepository = new TrackerStoreRepository(documentClient);
            var service = new TrackerStoreService(context, trackerStoreRepository);
            return service;
        }
    }
}
