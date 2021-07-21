using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Handlers
{
    public interface ILocationTracker
    {
        Task<string> GetCurrentLocationAsync(string registrationId);
        Task<IList<Location>> GetLocationsForDurationAsync(string registrationId, DateTime startTime, DateTime endTime);
        Task AddLocationAsync(TrackingRecord record);
    }
}
