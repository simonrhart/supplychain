using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Gateway.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories
{
    public interface ISmartContractsRepository
    {
        List<SoliditySmartContract> GetAllSmartContractsByName(SmartContractName name);

        Task Update(SoliditySmartContract contract);
    }
}
