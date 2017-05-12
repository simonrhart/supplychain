﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.SupplyChain.Cloud.Tracking.Contracts
{
    public interface IDeviceMovementServiceAgent
    {
        Task<List<TrackingDto>> GetTrackingUsingIdAsync(List<TrackerHashDto> trackerHashCollection,
            string deviceId);
    }
}
