using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.SupplyChain.Cloud.Administration.Contracts;
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

        public async Task InsertAsync(TrackerHashDto trackerHashDto)
        {
            if (trackerHashDto == null)
                throw new ArgumentNullException(nameof(trackerHashDto));

            await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentUri(_databaseName, _documentCollectionName, trackerHashDto.Id), trackerHashDto);
        }

        public List<TrackerHashDto> GetHashsByTime(string deviceId, DateTime from, DateTime to)
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<TrackerHashDto> smartContractQuery = _documentClient.CreateDocumentQuery<TrackerHashDto>(
                    UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), queryOptions)
                .Where(d => d.DeviceId == deviceId && d.TimeStamp >= from && d.TimeStamp <= to);

            return smartContractQuery.ToList();
        }

    }
}
