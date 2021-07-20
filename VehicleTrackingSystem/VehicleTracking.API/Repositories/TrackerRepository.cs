using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<TrackerRepository> _logger;
        public TrackerRepository(IConfiguration configuration, IDatabaseSettings databaseSettings, ILogger<TrackerRepository> logger)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

           _records  = database.GetCollection<TrackingRecord>(configuration.GetValue<string>("MongoDatabaseSettings:TrackerCollection"));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TrackingRecord> GetCurrentRecordAsync(string registrationId)
        {
            try
            {
                return await _records
                .Find(record => record.RegistrationId == registrationId)
                .SortByDescending(record => record.Time)
                .Limit(1)
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching data for the record with registration Id {registrationId}. Exception : {ex.Message}");
                throw ex;
            }
        }

        public async Task<List<TrackingRecord>> GetRecordsAsync(string registrationId, DateTime startTime, DateTime endTime)
        {
            try
            {
                return await _records
                 .Find(record => record.RegistrationId.Equals(registrationId)
            && record.Time.CompareTo(startTime) >= 0
            && record.Time.CompareTo(endTime) <= 0)
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching data for the record with registration Id {registrationId}. Exception : {ex.Message}");
                throw ex;
            }
        }

        public async Task AddRecordAsync(TrackingRecord record)
        {
            try
            {
                await _records.InsertOneAsync(record);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while saving data for the record {record.RegistrationId}. Exception : {ex.Message}");
                throw ex;
            }
        }
    }
}
