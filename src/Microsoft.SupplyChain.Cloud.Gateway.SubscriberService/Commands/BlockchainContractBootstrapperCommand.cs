﻿using System;
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
            var deviceMovementSmartContracts = _smartContractStoreServiceAgent.GetAllSmartContractsByName(SmartContractName.DeviceMovement).Result;

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
