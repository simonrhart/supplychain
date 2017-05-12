using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IBlockchainServiceAgent
    {
        Task DeploySmartContractAsync(SmartContractDto smartContract);
    }
}
