using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public class TrackerRepository : ITrackerRepository
    {
        private readonly IMongoCollection<TrackingRecord> _records;
        public TrackerRepository(IConfiguration configuration, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

           _records  = database.GetCollection<TrackingRecord>(configuration.GetValue<string>("MongoDatabaseSettings:TrackerCollection"));
        }

        public async Task<TrackingRecord> GetCurrentRecordAsync(string registrationId)
        { 
            return await _records
                .Find(record => record.RegistrationId == registrationId)
                .SortByDescending(record => record.Time)
                .Limit(1)
                .FirstOrDefaultAsync();
        }

        public async Task<List<TrackingRecord>> GetRecordsAsync(string registrationId, DateTime startTime, DateTime endTime)
        {
            return await _records
                 .Find(record => record.RegistrationId.Equals(registrationId)
            && record.Time.CompareTo(startTime) >= 0
            && record.Time.CompareTo(endTime) <= 0)
                 .ToListAsync();
        }

        public async Task AddRecordAsync(TrackingRecord record)
        {
            await _records.InsertOneAsync(record);
        }
    }
}
