using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IBlockchainServiceAgent
    {
        Task PublishAsync<TPayload>(TPayload payload);

        Task DeploySmartContractAsync(SoliditySmartContract smartContract);
    }
}
