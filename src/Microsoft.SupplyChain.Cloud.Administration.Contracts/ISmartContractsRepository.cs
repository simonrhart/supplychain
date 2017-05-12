using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Administration.Contracts
{
    public interface ISmartContractsRepository
    {
        Task<List<SmartContractDto>> GetAllSmartContractsByNameAsync(SmartContractName name);

        Task UpdateAsync(SmartContractDto contract);

        Task<SmartContractDto> GetLatestVersionSmartContractByNameAsync(SmartContractName name);
    }
}
