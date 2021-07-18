using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Repositories
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetAsync(string id);

        Task<Vehicle> GetVehicleUsingVehicleIdNumber(string vin);

        Task RegisterAsync(Vehicle vehicle);
    }
}
