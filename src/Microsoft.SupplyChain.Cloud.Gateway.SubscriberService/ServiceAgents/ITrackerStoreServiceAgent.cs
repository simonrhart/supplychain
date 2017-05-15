using System.Threading.Tasks;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;

namespace Microsoft.SupplyChain.Cloud.Gateway.SubscriberService.ServiceAgents
{
    public interface ITrackerStoreServiceAgent
    {
        Task PublishAsync(TrackerHashDto trackingHashDto);
    }
}
