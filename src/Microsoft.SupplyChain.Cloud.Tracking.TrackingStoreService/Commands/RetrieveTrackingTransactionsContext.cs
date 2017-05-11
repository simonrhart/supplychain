using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands
{
    public class RetrieveTrackingTransactionsContext : BaseContext
    {
        public RetrieveTrackingTransactionsContext(DateTime from, DateTime to, string deviceId, )
        {
        }
    }
}
