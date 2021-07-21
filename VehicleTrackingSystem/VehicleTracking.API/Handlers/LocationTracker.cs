using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Repositories;

namespace VehicleTracking.API.Handlers
{
    public class LocationTracker : ILocationTracker
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocationTracker> _logger;
        private readonly ITrackerRepository _trackerRepository;

        public LocationTracker(IConfiguration configuration, ILogger<LocationTracker> logger, ITrackerRepository trackerRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _trackerRepository = trackerRepository ?? throw new ArgumentNullException(nameof(trackerRepository));
        }

        public async Task<string> GetCurrentLocationAsync(string registrationId)
        {
            try
            {
                var record = await _trackerRepository.GetCurrentRecordAsync(registrationId);

                if (record != null)
                {
                    return await GetLocality(record.Location);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching current location for vehicle with registration Id {registrationId}.", ex.Message);
                throw ex;
            }
        }

        public async Task<IList<Location>> GetLocationsForDurationAsync(string registrationId, DateTime startTime, DateTime endTime)
        {
            try
            {
                var records = await _trackerRepository.GetRecordsAsync(registrationId, startTime, endTime);

                var locations = new List<Location>();

                records?.ForEach(tracking => locations.Add(tracking.Location));

                return locations;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching locations for a duration for vehicle with registration Id {registrationId}.", ex.Message);
                throw ex;
            }
        }

        public async Task AddLocationAsync(TrackingRecord record)
        {
            try
            {
                await _trackerRepository.AddRecordAsync(record);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while adding location for the vehicle with registration Id {record.RegistrationId}.", ex.Message);
                throw ex;
            }
        }

        private async Task<string> GetLocality(Location location)
        {
            try
            {
                var geoCodingApiUrl =
                string.Format(@"https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&result_type=sublocality_level_1&key={2}",
                location.Latitude, location.Longitude, _configuration.GetValue<string>("MapsApiKey"));
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, geoCodingApiUrl);

                var response = await client.SendAsync(request);

                if (response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    var data = JsonSerializer.Deserialize<GeoCodingResponse>(jsonResponse);

                    if (data?.Results != null && data?.Results.Count > 0)
                    {
                        var address = data.Results[0].FormattedAddress;

                        return address;
                    }
                    else
                    {
                        return "Invalid data";
                    }
                }
                else
                {
                    throw new Exception(message: $"Error while sending request to the Google Map API. Status Code : {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching locality for the coordinates {location.Latitude}, {location.Longitude}.", ex.Message);
                throw ex;
            }
        }
    }
}
