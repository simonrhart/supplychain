using System.Fabric;

namespace Microsoft.SupplyChain.Framework.ServiceFabric
{
    public interface IStatelessServiceContext
    {
        StatelessServiceContext Context { get; }
    }
}
