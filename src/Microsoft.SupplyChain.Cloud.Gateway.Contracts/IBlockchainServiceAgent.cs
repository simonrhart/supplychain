using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IBlockchainServiceAgent
    {
        Task DeploySmartContractAsync(SmartContractDto smartContract);
    }
}
