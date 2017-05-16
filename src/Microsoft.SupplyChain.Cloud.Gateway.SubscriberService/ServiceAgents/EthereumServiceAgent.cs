using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using ISmartContractStoreServiceAgent = Microsoft.SupplyChain.Cloud.Gateway.Contracts.ISmartContractStoreServiceAgent;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents
{
    public class EthereumServiceAgent : IBlockchainServiceAgent
    {
        private bool _disposed;
        private readonly Web3 _web3;
        private readonly ISmartContractStoreServiceAgent _smartContractServiceAgent;
        private readonly string _blockchainAdminAccount;
        private readonly string _blockchainAdminPassphrase;
      
        public EthereumServiceAgent(ISubscriberService subscriberService, ISmartContractStoreServiceAgent smartContractServiceAgent)
        {
            _smartContractServiceAgent = smartContractServiceAgent;

            var configurationPackage = subscriberService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
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

        public async Task DeploySmartContractAsync(SmartContractDto smartContract)
        {
            if (smartContract == null)
                throw new ArgumentNullException(nameof(smartContract));
         
            // unlock the admin account first for 120 seconds
            var unlockResult =
                await _web3.Personal.UnlockAccount.SendRequestAsync(_blockchainAdminAccount, _blockchainAdminPassphrase,
                    120);

            if (!unlockResult)
                throw new Exception(
                    $"Failed to unlock account {_blockchainAdminAccount} check you have the correct passphrase in the Service Fabric config.");

            string transactionsHash;
            try
            {
                transactionsHash =
                    await _web3.Eth.DeployContract.SendRequestAsync(smartContract.ByteCode, _blockchainAdminAccount,
                        new HexBigInteger(900000));
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to deploy smart contract {smartContract.Name} version {smartContract.Version}", ex);
            }

            var receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionsHash);

            // wait for the transaction (smart contract deploy) to be mined.
            while (receipt == null)
            {
                Thread.Sleep(5000);
                receipt = await _web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionsHash);
            }

            // now we have the contract address we need to update the documentDB record
            var contractAddress = receipt.ContractAddress;

            smartContract.Address = contractAddress;
            smartContract.IsDeployed = true;

            // now update the smart contract so we know it has been deployed along with the smart contract address.
            await _smartContractServiceAgent.UpdateAsync(smartContract);
        }
    }
}
