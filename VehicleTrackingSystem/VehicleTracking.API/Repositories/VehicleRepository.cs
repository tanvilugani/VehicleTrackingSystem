using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private IMongoCollection<Vehicle> _vehicles;
        public VehicleRepository(IConfiguration configuration, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _vehicles = database.GetCollection<Vehicle>(configuration.GetValue<string>("MongoDatabaseSettings:VehicleCollection"));
        }

        public async Task<Vehicle> GetAsync(string id)
        {
            return await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<Vehicle> GetVehicleUsingVehicleIdNumber(string vin)
        {
            return await _vehicles.Find(vehicle => vehicle.VehicleIdentificationNumber.Equals(vin)).FirstOrDefaultAsync();
        }

        public async Task RegisterAsync(Vehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
        }
    }
}
