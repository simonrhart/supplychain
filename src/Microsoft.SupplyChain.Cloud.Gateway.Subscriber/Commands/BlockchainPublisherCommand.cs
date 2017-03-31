using Microsoft.SupplyChain.Framework;
using Nethereum.Web3;
using System;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherCommand : BaseCommand<BlockchainPublisherContext>
    {
        private Web3 web3;

        public BlockchainPublisherCommand()
        {
            
        }

        protected override void DoExecute(BlockchainPublisherContext context)
        {
            
        }

        protected override void DoInitialize(BlockchainPublisherContext context)
        {
            // get all iot hub config data from the service fabric config package.
            var configurationPackage = context.FabricServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var iotHubSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            context.TransactionNodeVip = iotHubSection["TransactionNodeVip"].Value;
        
            web3 = new Web3(context.TransactionNodeVip);
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainPublisherContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
