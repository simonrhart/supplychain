using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackerStoreService.Repositories
{
    public class TrackerStoreRepository : ITrackerStoreRepository
    {
        private readonly ITrackerStoreService _trackerStoreService;
        private readonly DocumentClient _documentClient;
        private string _databaseName = "AdminDB";
        private string _documentCollectionName = "TrackerStoreCollection";

        public TrackerStoreRepository(ITrackerStoreService trackerStoreService)
        {
            _trackerStoreService = trackerStoreService;

            // read docDB from service fabric configuration package.
            var configurationPackage = _trackerStoreService.Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var documentDbSection = configurationPackage.Settings.Sections["DocumentDB"].Parameters;

            _documentClient = new DocumentClient(new Uri(documentDbSection["DocumentDBEndpointUri"].Value), documentDbSection["DocumentDBPrimaryKey"].Value);
            Task.Run(async () => await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName })).GetAwaiter().GetResult();
            Task.Run(async () => await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), new DocumentCollection { Id = _documentCollectionName })).GetAwaiter().GetResult();


        }


    }
}
