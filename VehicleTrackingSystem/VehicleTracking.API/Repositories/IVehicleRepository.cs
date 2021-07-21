using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public interface IVehicleRepository
    {
        Task<List<Vehicle>> GetVehiclesAsync();

        Task<Vehicle> GetAsync(string id);

        Task<Vehicle> GetVehicleUsingVehicleIdNumber(string vin);

        Task<bool> IsVehicleMapped(string vehicleIdNumber);

        Task<bool> IsDeviceMapped(string deviceId);

        Task RegisterVehicleAsync(Vehicle vehicle);
    }
}
