using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Commands;
using Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.Repositories;
using Microsoft.SupplyChain.Framework.Command;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents
{
    public class EthereumServiceAgent : IBlockchainServiceAgent
    {
        private bool _disposed;
        private readonly Web3 _web3;
        private readonly ISmartContractsRepository _smartContractsRepository;
        private readonly ISubscriberService _subscriberService;
        private readonly string _blockchainAdminAccount;
        private readonly string _blockchainAdminPassphrase;
        private string _contractAddress = null;
        private SoliditySmartContract _deviceMovementSmartContract;
        private Contract _contract;
        private Function _storeMovementFunction;

        public EthereumServiceAgent(ISubscriberService subscriberService, ISmartContractsRepository smartContractsRepository, IDeviceStoreService )
        {
            _smartContractsRepository = smartContractsRepository;
            _subscriberService = subscriberService;

            var configurationPackage = _subscriberService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var blockchainSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            var transactionNodeVip = blockchainSection["TransactionNodeVip"].Value;

            // this blockchain account is only used to send and public smart contracts, not to actually create telemetry transactions.
            _blockchainAdminAccount = blockchainSection["BlockchainAdminAccount"].Value;
            _blockchainAdminPassphrase = blockchainSection["BlockchainAdminPassphrase"].Value;

            if (string.IsNullOrEmpty(transactionNodeVip))
                throw new Exception("TransactionNodeVip is not set in Service Fabric configuration package.");

            if (string.IsNullOrEmpty(_blockchainAdminAccount))
                throw new Exception("BlockchainAdminAccount is not set in Service Fabric configuration package.");

            if (string.IsNullOrEmpty(_blockchainAdminPassphrase))
                throw new Exception("BlockchainAdminPassphrase is not set in Service Fabric configuration package.");

            _web3 = new Web3(transactionNodeVip);
        }

        public async Task PublishAsync<TPayload>(TPayload payload)
        {
            // publish the telemetry on the blockchain. Firstly check if we have a reference to the contract.
            if (_contract == null)
            {
                // get the latest smart contract version to invoke.
                _deviceMovementSmartContract = _smartContractsRepository.GetLatestSmartContractByName(SmartContractName.DeviceMovement);

                // if it's been removed since we bootstrapped the application, redeploy it.
                if (!_deviceMovementSmartContract.IsDeployed)
                    await DeploySmartContractAsync(_deviceMovementSmartContract);

                // now load the contract using the contract address

                _contract = _web3.Eth.GetContract(_deviceMovementSmartContract.Abi,
                    _deviceMovementSmartContract.Address);

                _storeMovementFunction = _contract.GetFunction("StoreMovement");
            }


            
           
        }

        public async Task DeploySmartContractAsync(SoliditySmartContract smartContract)
        {
            // unlock the admin account first for 120 seconds
            var unlockResult = await _web3.Personal.UnlockAccount.SendRequestAsync(_blockchainAdminAccount, _blockchainAdminPassphrase, 120);

            var transactionsHash =
              await _web3.Eth.DeployContract.SendRequestAsync(smartContract.ByteCode, _blockchainAdminAccount, new HexBigInteger(900000));

            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionsHash);

            // wait for the transaction (smart contract deploy) to be mined.
            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionsHash);
            }

            // now we have the contract address we need to update the documentDB record
            _contractAddress = receipt.ContractAddress;

            smartContract.Address = _contractAddress;
            smartContract.IsDeployed = true;

            // now update the smart contract so we know it has been deployed along with the smart contract address.
            await _smartContractsRepository.UpdateAsync(smartContract);
        }
    }
}
