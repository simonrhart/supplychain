using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class Subscriber : StatelessService, ISubscriber
    {   
        public Subscriber(StatelessServiceContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
           
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // firstly, kick off the blockchain bootstrapper command. This ensures blockchain is ready for prime time
            ICommandAbstractFactory commandAbstractFactory = ServiceLocator.Current.GetInstance<ICommandAbstractFactory>();
            await commandAbstractFactory.ExecuteCommandAsync(new BlockchainContractBootstrapperContext());

            // execute the iot hub subscriber command. This is where things start.           
            await commandAbstractFactory.ExecuteCommandAsync(new IoTHubSubscriberContext());

            long iterations = 0;
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}
