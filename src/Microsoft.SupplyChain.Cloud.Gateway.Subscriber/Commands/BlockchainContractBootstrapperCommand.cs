using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories;
using Microsoft.SupplyChain.Framework;
using Nethereum.Web3;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperCommand : BaseCommand<BlockchainContractBootstrapperContext>
    {
        private readonly IBlockchainServiceAgent _blockchainServiceAgent;
        private readonly ISmartContractsRepository _smartContractsRepository;

        public BlockchainContractBootstrapperCommand(ISmartContractsRepository smartContractsRepository, IBlockchainServiceAgent blockchainServiceAgent)
        {
            _blockchainServiceAgent = blockchainServiceAgent;
            _smartContractsRepository = smartContractsRepository;
        }

        protected override async Task DoExecute(BlockchainContractBootstrapperContext context)
        {
            // now get whether the smart contract is the latest one deployed or not.
            var deviceMovementSmartContracts = _smartContractsRepository.GetAllSmartContractsByName(SmartContractName.DeviceMovement);

            if (deviceMovementSmartContracts.Count == 0)
                throw new Exception("No smart contracts found");

            // now check if we have the latest and whether we need to deploy any smart contracts.
            var sortedContracts = deviceMovementSmartContracts.OrderByDescending(v => v.Version);

            var contract = sortedContracts.FirstOrDefault();            

            if (!contract.IsDeployed)
            {
                // then we need to deploy this version of the smart contract to the blockchain
                await _blockchainServiceAgent.DeploySmartContractAsync(contract);
            }
        }
       
        protected override ExceptionAction HandleError(BlockchainContractBootstrapperContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
