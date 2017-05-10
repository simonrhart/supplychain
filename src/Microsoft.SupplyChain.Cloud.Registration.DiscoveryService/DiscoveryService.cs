using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Controllers;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Framework.Mvc;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class DiscoveryService : StatelessService, IDiscoveryService
    {
        public DiscoveryService(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(StartupWithWindsor.ConfigureApp,
                    serviceContext, ServiceEventSource.Current, "ServiceEndpoint")),
            };
        }

    }
}
