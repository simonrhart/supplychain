using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Gateway.Subscriber
{
    public interface ISubscriber
    {
        StatelessServiceContext Context { get; }
    }
}
