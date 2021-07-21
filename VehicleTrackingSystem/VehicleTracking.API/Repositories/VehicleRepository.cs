using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;
        private readonly ILogger<VehicleRepository> _logger;
        public VehicleRepository(IConfiguration configuration, IDatabaseSettings databaseSettings, ILogger<VehicleRepository> logger)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _vehicles = database.GetCollection<Vehicle>(configuration.GetValue<string>("MongoDatabaseSettings:VehicleCollection"));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                return await _vehicles.Find(vehicle => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching vehicles.");
                throw ex;
            }
        }

        public async Task<Vehicle> GetAsync(string id)
        {
            try
            {
                return await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching data for vehicle {id}.", ex.Message);
                throw ex;
            }
        }

        public async Task<Vehicle> GetVehicleUsingVehicleIdNumber(string vin)
        {
            try
            {
                return await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(vin)).FirstOrDefaultAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching data for vehicle {vin} using VIN.", ex.Message);
                throw ex;
            }
        }

        public async Task<bool> IsDeviceMapped(string deviceId)
        {
            try
            {
                var result = await _vehicles.Find(vehicle => vehicle.MappedDeviceId.Equals(deviceId) && vehicle.IsActive == true).FirstOrDefaultAsync();

                if (result != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching if device is mapped or not.", ex.Message);
                throw ex;
            }

            return false;
        }

        public async Task<bool> IsVehicleMapped(string vehicleIdNumber)
        {
            try
            {
                var result = await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(vehicleIdNumber) && vehicle.IsActive == true)
                    .FirstOrDefaultAsync();

                if (result != null)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching if vehicle is mapped or not.", ex.Message);
                throw ex;
            }

            return false;
        }

        public async Task RegisterVehicleAsync(Vehicle vehicle)
        {
            try
            {
                await _vehicles.InsertOneAsync(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while saveing data for vehicle {vehicle.VehicleIdentificationNumber}.", ex.Message);
                throw ex;
            }
        }
    }
}
