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

            try
            {
                await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), trackerHashDto);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    $"Failed to add TrackerHashDto to database {_databaseName} in collection {_documentCollectionName}",
                    ex);
            }
        }

        public List<TrackerHashDto> GetHashsByTime(DateTime from, DateTime to, string deviceId = "")
        {
            FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
            IQueryable<TrackerHashDto> smartContractQuery;

            if (string.IsNullOrEmpty(deviceId))
            {
                smartContractQuery = _documentClient.CreateDocumentQuery<TrackerHashDto>(
                        UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), queryOptions)
                    .Where(d => d.TimeStamp >= from && d.TimeStamp <= to);
            }
            else
            {
                smartContractQuery = _documentClient.CreateDocumentQuery<TrackerHashDto>(
                        UriFactory.CreateDocumentCollectionUri(_databaseName, _documentCollectionName), queryOptions)
                    .Where(d => d.DeviceId == deviceId && d.TimeStamp >= from && d.TimeStamp <= to);
            }
            return smartContractQuery.ToList();
        }

    }
}
