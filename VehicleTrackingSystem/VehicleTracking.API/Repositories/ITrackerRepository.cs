using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public interface ITrackerRepository
    {
        Task<TrackingRecord> GetCurrentRecordAsync(string registrationId);
        Task<List<TrackingRecord>> GetRecordsAsync(string registrationId, DateTime startTime, DateTime endTime);
        Task AddRecordAsync(TrackingRecord record);
    }
}
