using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Microsoft.SupplyChain.Framework;

namespace Microsoft.SupplyChain.Cloud.Registration.DiscoveryService.Configuration
{
    public interface IContainerBuilder
    {
        void BuildCommands();
        IWindsorContainer Container { get; }

        IServiceLocator Build();
    }
}
