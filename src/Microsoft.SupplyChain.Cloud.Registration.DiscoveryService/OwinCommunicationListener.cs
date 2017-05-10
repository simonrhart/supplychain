using System;
using System.Diagnostics;
using System.Fabric;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Owin;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService
{
    internal class OwinCommunicationListener : ICommunicationListener
    {
        private readonly ServiceEventSource _eventSource;
        private readonly Action<IAppBuilder> _startup;
        private readonly ServiceContext _serviceContext;
        private readonly string _endpointName;
        private readonly string _appRoot;

        private IDisposable _webApp;
        private string _publishAddress;
        private string _listeningAddress;

        public OwinCommunicationListener(Action<IAppBuilder> startup, ServiceContext serviceContext, ServiceEventSource eventSource, string endpointName)
            : this(startup, serviceContext, eventSource, endpointName, null)
        {
        }

        public OwinCommunicationListener(Action<IAppBuilder> startup, ServiceContext serviceContext, ServiceEventSource eventSource, string endpointName, string appRoot)
        {
            this._startup = startup ?? throw new ArgumentNullException(nameof(startup));
            this._serviceContext = serviceContext ?? throw new ArgumentNullException(nameof(serviceContext));
            this._endpointName = endpointName ?? throw new ArgumentNullException(nameof(endpointName));
            this._eventSource = eventSource ?? throw new ArgumentNullException(nameof(eventSource));
            this._appRoot = appRoot;
        }

        public bool ListenOnSecondary { get; set; }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceEndpoint = this._serviceContext.CodePackageActivationContext.GetEndpoint(this._endpointName);
            int port = serviceEndpoint.Port;

            if (this._serviceContext is StatefulServiceContext)
            {
                StatefulServiceContext statefulServiceContext = this._serviceContext as StatefulServiceContext;

                this._listeningAddress = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}{2}/{3}/{4}",
                    port,
                    string.IsNullOrWhiteSpace(this._appRoot)
                        ? string.Empty
                        : this._appRoot.TrimEnd('/') + '/',
                    statefulServiceContext.PartitionId,
                    statefulServiceContext.ReplicaId,
                    Guid.NewGuid());
            }
            else if (this._serviceContext is StatelessServiceContext)
            {
                this._listeningAddress = string.Format(
                    CultureInfo.InvariantCulture,
                    "http://+:{0}/{1}",
                    port,
                    string.IsNullOrWhiteSpace(this._appRoot)
                        ? string.Empty
                        : this._appRoot.TrimEnd('/') + '/');
            }
            else
            {
                throw new InvalidOperationException();
            }

            this._publishAddress = this._listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);

            try
            {
                this._eventSource.ServiceMessage(this._serviceContext, "Starting web server on " + this._listeningAddress);

                this._webApp = WebApp.Start(this._listeningAddress, appBuilder => this._startup.Invoke(appBuilder));

                this._eventSource.ServiceMessage(this._serviceContext, "Listening on " + this._publishAddress);

                return Task.FromResult(this._publishAddress);
            }
            catch (Exception ex)
            {
                this._eventSource.ServiceMessage(this._serviceContext, "Web server failed to open. " + ex.ToString());

                this.StopWebServer();

                throw;
            }
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            this._eventSource.ServiceMessage(this._serviceContext, "Closing web server");

            this.StopWebServer();

            return Task.FromResult(true);
        }

        public void Abort()
        {
            this._eventSource.ServiceMessage(this._serviceContext, "Aborting web server");

            this.StopWebServer();
        }

        private void StopWebServer()
        {
            if (this._webApp != null)
            {
                try
                {
                    this._webApp.Dispose();
                }
                catch (ObjectDisposedException)
                {
                    // no-op
                }
            }
        }
    }
}
