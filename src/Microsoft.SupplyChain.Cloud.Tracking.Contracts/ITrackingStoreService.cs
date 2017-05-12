using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.SupplyChain.Framework.ServiceFabric;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface ITrackingStoreService : IService
    {
        Task PublishAsync(TrackerHashDto trackerHashDto);
    }
}
