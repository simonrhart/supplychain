using Microsoft.SupplyChain.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories
{
    public interface ISmartContractsRepository
    {
        List<SoliditySmartContract> GetAllSmartContractsByName(SmartContractName name);

        Task Update(SoliditySmartContract contract);
    }
}
