using System;
using Microsoft.SupplyChain.Services.Contracts;
using Nethereum.Web3;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.ServiceAgents
{
    public class EthereumServiceAgent : IBlockchainServiceAgent
    {
        private bool _disposed;
        private Web3 _web3;
        private ISubscriber _subscriber;
        private string _blockchainAdminAccount;
        private string _blockchainAdminPassphrase;

        public EthereumServiceAgent(ISubscriber subscriber)
        {
            _subscriber = subscriber;

            var configurationPackage = _subscriber.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var blockchainSection = configurationPackage.Settings.Sections["Blockchain"].Parameters;
            var transactionNodeVip = blockchainSection["TransactionNodeVip"].Value;
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

        public void Publish<TPayload>(TPayload payload)
        {

        }

        public async Task DeploySmartContractAsync(SoliditySmartContract smartContract)
        {
            // unlock the admin account first for 120 seconds
            var unlockResult = await _web3.Personal.UnlockAccount.SendRequestAsync(_blockchainAdminAccount, _blockchainAdminPassphrase, 120);

            var transactionsHash =
              await _web3.Eth.DeployContract.SendRequestAsync(smartContract.ByteCode, _blockchainAdminAccount, new HexBigInteger(900000));
        }



    }
}
