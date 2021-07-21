using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;


namespace VehicleTracking.API.Handlers
{
    public interface IVehicleHandler
    {
        Task<List<Vehicle>> GetVehiclesAsync();

        Task<(bool, string)> RegisterVehicleAsync(Vehicle vehicle);
    }
}
