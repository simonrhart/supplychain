using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface ITrackingStoreService : IService
    {
        Task PublishAsync(TrackerHashDto trackerHashDto);
    }
}
