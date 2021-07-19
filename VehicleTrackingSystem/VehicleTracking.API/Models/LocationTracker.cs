using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Repositories;

namespace VehicleTracking.API.Models
{
    public class LocationTracker : ILocationTracker
    {
        private readonly ITrackerRepository _trackerRepository;

        public LocationTracker(ITrackerRepository trackerRepository)
        {
            _trackerRepository = trackerRepository ?? throw new ArgumentNullException(nameof(trackerRepository));
        }

        public async Task<Location> GetCurrentLocationAsync(string registrationId)
        {
            var record = await _trackerRepository.GetCurrentRecordAsync(registrationId);

            return record?.Location;
        }

        public async Task<IList<Location>> GetLocationsDuringDurationAsync(string registrationId, DateTime startTime, DateTime endTime)
        {
            var records = await _trackerRepository.GetRecordsAsync(registrationId, startTime, endTime);

            var locations = new List<Location>();

            records?.ForEach(tracking => locations.Add(tracking.Location));

            return locations;
        }

        public async Task AddLocationAsync(TrackingRecord record)
        {
            await _trackerRepository.AddRecordAsync(record);
        }
    }
}
