using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IBlockchainServiceAgent<in TPayload>
    {
        Task PublishAsync(TPayload payload);

        Task DeploySmartContractAsync(SoliditySmartContract smartContract);
    }
}
