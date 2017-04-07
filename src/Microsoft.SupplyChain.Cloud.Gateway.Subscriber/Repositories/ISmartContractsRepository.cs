using Microsoft.SupplyChain.Services.Contracts;
using System.Collections.Generic;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories
{
    public interface ISmartContractsRepository
    {
        List<SoliditySmartContract> GetAllSmartContractsByName(SmartContractName name);

        void Update(SoliditySmartContract contract);
    }
}
