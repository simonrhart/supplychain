using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface ITrackerStoreRepository
    {
        Task InsertAsync(TrackerHashDto trackerHashDto);
        List<TrackerHashDto> GetHashsByTime(string deviceId, DateTime from, DateTime to);
    }
}
