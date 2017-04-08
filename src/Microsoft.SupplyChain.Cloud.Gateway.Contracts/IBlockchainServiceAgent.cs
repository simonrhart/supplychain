using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IBlockchainServiceAgent
    {
        void Publish<TPayload>(TPayload payload);

        Task DeploySmartContractAsync(SoliditySmartContract smartContract);
    }
}
