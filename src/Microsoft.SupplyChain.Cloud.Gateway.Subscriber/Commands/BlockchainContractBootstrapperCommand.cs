using Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories;
using Microsoft.SupplyChain.Framework;
using Microsoft.SupplyChain.Services.Contracts;
using Nethereum.Web3;
using System;
using System.Linq;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Commands
{
    public class BlockchainContractBootstrapperCommand : BaseCommand<BlockchainContractBootstrapperContext>
    {
        private IBlockchainServiceAgent _blockchainServiceAgent;
        private ISmartContractsRepository _smartContractsRepository;

        public BlockchainContractBootstrapperCommand(ISmartContractsRepository smartContractsRepository, IBlockchainServiceAgent blockchainServiceAgent)
        {
            _blockchainServiceAgent = blockchainServiceAgent;
            _smartContractsRepository = smartContractsRepository;
        }

        protected override void DoExecute(BlockchainContractBootstrapperContext context)
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
                _blockchainServiceAgent.DeploySmartContract(contract);

            }

            
        }

        protected override void DoInitialize(BlockchainContractBootstrapperContext context)
        {            
           
            base.DoInitialize(context);
        }

        protected override ExceptionAction HandleError(BlockchainContractBootstrapperContext context, Exception exception)
        {
            return ExceptionAction.Rethrow;
        }
    }
}
