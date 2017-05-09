using System;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Repositories;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Tests.ServiceAgents
{
    [TestClass]
    public class EthereumDeviceMovementServiceAgentTests
    {
        [TestMethod]
        public void ShouldThrowOnNullSubscriberService()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new EthereumDeviceMovementServiceAgent(null,
                (ISmartContractsRepository) new object(),
                new object() as IDeviceStoreServiceAgent, new object() as IBlockchainServiceAgent,
                (ITrackerStoreServiceAgent) new object()));

        }
    }
}
