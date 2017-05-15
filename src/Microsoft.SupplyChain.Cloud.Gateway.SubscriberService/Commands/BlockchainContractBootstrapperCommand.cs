using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Commands
{
    public class BlockchainContractBootstrapperCommand : BaseCommand<BlockchainContractBootstrapperContext>
    {
        private readonly IBlockchainServiceAgent _blockchainServiceAgent;
        private readonly ISmartContractStoreServiceAgent _smartContractStoreServiceAgent;

        public BlockchainContractBootstrapperCommand(ISmartContractStoreServiceAgent smartContractStoreServiceAgent, IBlockchainServiceAgent blockchainServiceAgent)
        {
            _blockchainServiceAgent = blockchainServiceAgent;
            _smartContractStoreServiceAgent = smartContractStoreServiceAgent;
        }

        protected override async Task DoExecuteAsync(BlockchainContractBootstrapperContext context)
        {
            // now get whether the smart contract is the latest one deployed or not.
            var deviceMovementSmartContracts =
                await _smartContractStoreServiceAgent.GetLatestVersionSmartContractByName(SmartContractName.DeviceMovement);
            
            
            if (!deviceMovementSmartContracts.IsDeployed)
            {
                // then we need to deploy this version of the smart contract to the blockchain
                await _blockchainServiceAgent.DeploySmartContractAsync(deviceMovementSmartContracts);
            }
        }
       
        protected override ExceptionAction HandleError(BlockchainContractBootstrapperContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
