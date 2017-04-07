using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using Microsoft.SupplyChain.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber.Repositories
{
    public class SmartContractsRepository : ISmartContractsRepository
    {
        private ISubscriber _subscriber;
        private DocumentClient _documentClient;
        private string _databaseName = "AdminDB";
        private string _documentCollectionName = "SmartContractsCollection";
               
        public SmartContractsRepository(ISubscriber subscriber)
        {
            _subscriber = subscriber;
            // read docDB from service fabric configuration package.
            var configurationPackage = _subscriber.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var documentDBSection = configurationPackage.Settings.Sections["DocumentDB"].Parameters;                 
        
            _documentClient = new DocumentClient(new Uri(documentDBSection["DocumentDBEndpointUri"].Value), documentDBSection["DocumentDBPrimaryKey"].Value);
            Task.Run(async () => await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName })).GetAwaiter().GetResult();
            Task.Run(async () => await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), new DocumentCollection { Id = _documentCollectionName })).GetAwaiter().GetResult();
                     


        }

        public List<SoliditySmartContract> GetAllSmartContractsByName(SmartContractName name)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };

            IQueryable<SoliditySmartContract> smartContractQuery = _documentClient.CreateDocumentQuery<SoliditySmartContract>(
              UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), queryOptions)
              .Where(d => d.Name == name);

            return smartContractQuery.ToList();
        }

        public void AddContract()
        {
            // add dummy smart contract

            SoliditySmartContract test = new SoliditySmartContract();
            test.Name = SmartContractName.DeviceMovement;
            test.Id = string.Format("{0}.{1}", SmartContractName.DeviceMovement.ToString(), "1");
            test.Version = 1.0;
            test.IsDeployed = false;
            test.ByteCode = "6060604052341561000c57fe5b5b6105218061001c6000396000f300606060405263ffffffff60e060020a6000350416637ebf139a811461002c5780638fc7a6921461015d575bfe5b341561003457fe5b610042600435602435610207565b60408051908101839052600160a060020a038216606082015260808082528554600260001961010060018416150201909116049082018190528190602082019060a0830190889080156100d65780601f106100ab576101008083540402835291602001916100d6565b820191906000526020600020905b8154815290600101906020018083116100b957829003601f168201915b505083810382528654600260001961010060018416150201909116048082526020909101908790801561014a5780601f1061011f5761010080835404028352916020019161014a565b820191906000526020600020905b81548152906001019060200180831161012d57829003601f168201915b5050965050505050505060405180910390f35b341561016557fe5b60408051602060046024803582810135601f81018590048502860185019096528585526101f3958335959394604494939290920191819084018382808284375050604080516020601f89358b01803591820183900483028401830190945280835297999881019791965091820194509250829150840183828082843750949650509335935061025492505050565b604080519115158252519081900360200190f35b60006020528160005260406000208181548110151561022257fe5b906000526020600020906004020160005b506002810154600382015491935060018401925090600160a060020a031684565b600061025e610336565b50604080516080810182528581526020808201869052818301859052600160a060020a0333166060830152600088815290819052919091208054600181016102a6838261036b565b916000526020600020906004020160005b50825180518492916102ce9183916020019061039d565b5060208281015180516102e7926001850192019061039d565b50604082015160028201556060909101516003909101805473ffffffffffffffffffffffffffffffffffffffff1916600160a060020a0390921691909117905550600191505b50949350505050565b60806040519081016040528061034a61041c565b815260200161035761041c565b815260006020820181905260409091015290565b81548183558181151161039757600402816004028360005260206000209182019101610397919061042e565b5b505050565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f106103de57805160ff191683800117855561040b565b8280016001018555821561040b579182015b8281111561040b5782518255916020019190600101906103f0565b5b5061041892915061048c565b5090565b60408051602081019091526000815290565b61048991905b8082111561041857600061044882826104ad565b6104566001830160006104ad565b506000600282015560038101805473ffffffffffffffffffffffffffffffffffffffff19169055600401610434565b5090565b90565b61048991905b808211156104185760008155600101610492565b5090565b90565b50805460018160011615610100020316600290046000825580601f106104d357506104f1565b601f0160209004906000526020600020908101906104f1919061048c565b5b505600a165627a7a723058208987c9adf30b9190ee099dd8cc9135ba6410847068a1e0b1b3d68f6a5b74c71d0029";
            test.Abi = "[{\"constant\":true,\"inputs\":[{\"name\":\"\",\"type\":\"bytes32\"},{\"name\":\"\",\"type\":\"uint256\"}],\"name\":\"telemetryCollection\",\"outputs\":[{\"name\":\"lat\",\"type\":\"string\"},{\"name\":\"long\",\"type\":\"string\"},{\"name\":\"temperatureInCelcius\",\"type\":\"int256\"},{\"name\":\"sender\",\"type\":\"address\"}],\"payable\":false,\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"key\",\"type\":\"bytes32\"},{\"name\":\"lat\",\"type\":\"string\"},{\"name\":\"long\",\"type\":\"string\"},{\"name\":\"temperatureInCelcius\",\"type\":\"int256\"}],\"name\":\"StoreMovement\",\"outputs\":[{\"name\":\"success\",\"type\":\"bool\"}],\"payable\":false,\"type\":\"function\"}]";

            Task.Run(async () => await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), test)).GetAwaiter().GetResult();
        }      


    }
}
