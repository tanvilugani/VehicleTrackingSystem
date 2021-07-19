using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VehicleTracking.API.Models
{
    public interface ILocationTracker
    {
        public Task<Location> GetCurrentLocationAsync(string registrationId);
        public Task<IList<Location>> GetLocationsDuringDurationAsync(string registrationId, DateTime startTime, DateTime endTime);
        public Task AddLocationAsync(TrackingRecord record);
    }
}
