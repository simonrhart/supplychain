using Microsoft.SupplyChain.Framework;
using System;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainPublisherCommand : BaseCommand<BlockchainPublisherContext>
    {
        private readonly ICommand<BlockchainContractBootstrapperContext> _blockchainContractBootstrapperCommand;
        private IBlockchainServiceAgent _blockchainServiceAgent;

        public BlockchainPublisherCommand(ICommand<BlockchainContractBootstrapperContext> blockchainContractBootstrapperCommand, IBlockchainServiceAgent blockchainServiceAgent)
        {
            _blockchainContractBootstrapperCommand = blockchainContractBootstrapperCommand;
            _blockchainServiceAgent = blockchainServiceAgent;            
        }
        
        protected override async Task DoExecute(BlockchainPublisherContext context)
        {
           
        }        

        protected override void DoInitialize(BlockchainPublisherContext context)
        {
            var configurationPackage = context.StatelessServiceInstance.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");

            var iotHubSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            context.TransactionNodeVip = iotHubSection["TransactionNodeVip"].Value;

            // before we can do anything we need to bootstrap the smart contract.
            BlockchainContractBootstrapperContext bootStrapperContext = new BlockchainContractBootstrapperContext();
            _blockchainContractBootstrapperCommand.Execute(bootStrapperContext);

            base.DoInitialize(context);
           
          
        }

        protected override ExceptionAction HandleError(BlockchainPublisherContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }

        protected override TearDownAction HandleTearDown()
        {
            // executing tear down on dispose supresses this command as it will repeatedely be called
            return TearDownAction.OnDispose;
        }
    }
}
