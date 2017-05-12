using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace Microsoft.SupplyChain.Cloud.Administration.SmartContractStoreService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class SmartContractStoreService : StatelessService, ISmartContractStoreService
    {
        private readonly ISmartContractsRepository _smartContractsRepository;

        public SmartContractStoreService(StatelessServiceContext context, ISmartContractsRepository smartContractsRepository)
            : base(context)
        {
            _smartContractsRepository = smartContractsRepository;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[] { new ServiceInstanceListener(this.CreateServiceRemotingListener) };
        }


        public List<SmartContractDto> GetAllSmartContractsByName(SmartContractName name)
        {
            return _smartContractsRepository.GetAllSmartContractsByName(name);
        }

        public Task UpdateAsync(SmartContractDto contract)
        {
            return _smartContractsRepository.UpdateAsync(contract);
        }

        public SmartContractDto GetLatestVersionSmartContractByName(SmartContractName name)
        {
            return _smartContractsRepository.GetLatestVersionSmartContractByName(name);
        }
    }
}
