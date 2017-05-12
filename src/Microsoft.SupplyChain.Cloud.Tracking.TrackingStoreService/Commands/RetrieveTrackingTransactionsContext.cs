using System;
using System.Collections.Generic;
using Microsoft.SupplyChain.Cloud.Tracking.Contracts;
using Microsoft.SupplyChain.Framework.Command;

namespace Microsoft.SupplyChain.Cloud.Tracking.TrackingStoreService.Commands
{
    public class RetrieveTrackingTransactionsContext : BaseContext
    {
        public RetrieveTrackingTransactionsContext(DateTime from, DateTime to, string deviceId)
        {
            From = from;
            To = to;
            DeviceId = deviceId;
            TrackingCollection = new List<TrackingDto>();
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public string DeviceId { get; }

        public List<TrackingDto> TrackingCollection { get; set; }
    }
}
