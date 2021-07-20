using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
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

        public async Task<Vehicle> GetAsync(string id)
        {
            try
            {
                return await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(id)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching data for vehicle {id}. Exception : {ex.Message}");
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
                _logger.LogError($"Error while fetching data for vehicle {vin} using VIN. Exception : {ex.Message}");
                throw ex;
            }
        }

        public async Task RegisterAsync(Vehicle vehicle)
        {
            try
            {
                await _vehicles.InsertOneAsync(vehicle);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while saveing data for vehicle {vehicle.VehicleIdentificationNumber}. Exception : {ex.Message}");
                throw ex;
            }
        }
    }
}
