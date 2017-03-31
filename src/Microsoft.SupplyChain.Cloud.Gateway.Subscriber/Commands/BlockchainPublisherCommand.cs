using Microsoft.SupplyChain.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherCommand : BaseCommand<BlockchainPublisherContext>
    {
        private ICommand<BlockchainContractBootstrapperContext> _blockchainContractBootstrapperCommand;

        public BlockchainPublisherCommand(ICommand<BlockchainContractBootstrapperContext> blockchainContractBootstrapperCommand)
        {
            _blockchainContractBootstrapperCommand = blockchainContractBootstrapperCommand;
        }

        protected override void DoExecute(BlockchainPublisherContext context)
        {
           
        }

        protected override void DoInitialize(BlockchainPublisherContext context)
        {
            var configurationPackage = context.StatelessServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var iotHubSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            context.TransactionNodeVip = iotHubSection["TransactionNodeVip"].Value;

            BlockchainContractBootstrapperContext bootStrapperContext = new BlockchainContractBootstrapperContext(context.StatelessServiceInstance, context.TransactionNodeVip);
            _blockchainContractBootstrapperCommand.Execute(bootStrapperContext);

            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainPublisherContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
