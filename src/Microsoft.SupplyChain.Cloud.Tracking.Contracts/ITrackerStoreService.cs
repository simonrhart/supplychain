using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.SupplyChain.Framework.ServiceFabric;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface ITrackerStoreService : IService, IStatelessServiceContext
    {
        Task Publish(TrackerHashDto trackerHashDto);
    }
}
