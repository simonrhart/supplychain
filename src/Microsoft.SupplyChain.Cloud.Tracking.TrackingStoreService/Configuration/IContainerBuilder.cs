using Castle.Windsor;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Configuration
{
    public interface IContainerBuilder
    {
        void BuildCommands();
        IWindsorContainer Container { get; }

        IServiceLocator Build();
    }
}
