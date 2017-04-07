using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Services.Contracts
{
    public interface IBlockchainServiceAgent
    {
        void Publish<TPayload>(TPayload payload);

        Task DeploySmartContractAsync(SoliditySmartContract smartContract);
    }
}
