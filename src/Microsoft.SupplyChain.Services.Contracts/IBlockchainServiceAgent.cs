using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Services.Contracts
{
    public interface IBlockchainServiceAgent : IDisposable
    {
        void Publish()
    }
}
