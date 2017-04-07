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
            // read docDB from service fabric configuration package.
            var configurationPackage = _subscriber.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var documentDBSection = configurationPackage.Settings.Sections["DocumentDB"].Parameters;
                   
            _subscriber = subscriber;
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


    }
}
