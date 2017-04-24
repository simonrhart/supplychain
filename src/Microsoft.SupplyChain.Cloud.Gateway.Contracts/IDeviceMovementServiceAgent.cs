using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Contracts
{
    public interface IDeviceMovementServiceAgent
    {
        Task PublishAsync(Sensor payload);
    }
}
