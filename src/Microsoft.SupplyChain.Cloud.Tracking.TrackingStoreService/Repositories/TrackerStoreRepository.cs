using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Repositories
{
    public class TrackerStoreRepository : ITrackerStoreRepository
    {
        private readonly DocumentClient _documentClient;
        private string _databaseName = "TrackerDB";
        private string _documentCollectionName = "TrackerStoreCollection";

        public TrackerStoreRepository(DocumentClient documentClient)
        {
            _documentClient = documentClient;

            // read docDB from service fabric configuration package.
            Task.Run(async () => await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName })).GetAwaiter().GetResult();
            Task.Run(async () => await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), new DocumentCollection { Id = _documentCollectionName })).GetAwaiter().GetResult();
        }

        public async Task UpdateAsync(TrackerHashDto trackerHashDto)
        {
            if (trackerHashDto == null)
                throw new ArgumentNullException(nameof(trackerHashDto));

            await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _documentCollectionName, trackerHashDto.Id), trackerHashDto);
        }

    }
}
