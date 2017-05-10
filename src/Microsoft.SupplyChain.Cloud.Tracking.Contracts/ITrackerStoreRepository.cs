using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface ITrackerStoreRepository
    {
        Task UpdateAsync(TrackerHashDto trackerHashDto);
    }
}
