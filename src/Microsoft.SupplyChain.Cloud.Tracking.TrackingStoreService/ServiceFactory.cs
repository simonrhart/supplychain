using System;
using System.Fabric;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.Azure.Documents.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Repositories;
using Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.ServiceAgents;
using Microsoft.SupplyChain.Framework;
using Nethereum.Web3;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService
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

            var blockchainSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            var transactionNodeVip = blockchainSection["TransactionNodeVip"].Value;
            var blockchainAdminAccount = blockchainSection["BlockchainAdminAccount"].Value;
            var blockchainAdminAccountPassphrase = blockchainSection["BlockchainAdminPassphrase"].Value;

            var documentClient = new DocumentClient(new Uri(uri), documentDbPrimaryKey);
            
            ITrackerStoreRepository trackerStoreRepository = new TrackerStoreRepository(documentClient);
            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<ITrackerStoreRepository>().Instance(trackerStoreRepository));

            Web3 web3 = new Web3(transactionNodeVip);
            IDeviceMovementServiceAgent deviceMovementServiceAgent = new EthereumDeviceMovementServiceAgent(web3, blockchainAdminAccount, blockchainAdminAccountPassphrase, ServiceLocator.Current.GetInstance<ISmartContractStoreServiceAgent>());

            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<IDeviceMovementServiceAgent>()
                .Instance(deviceMovementServiceAgent)
                .Interceptors(InterceptorReference.ForKey("ConsoleInterceptor")).Anywhere
                .LifestyleTransient());

            var service = new TrackingStoreService(context, trackerStoreRepository);
            ServiceLocator.Current.GetInstance<IWindsorContainer>().Register(Component.For<ITrackingStoreService>().Instance(service));
            return service;
        }
    }
}
