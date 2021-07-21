using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;


namespace VehicleTracking.API.Handlers
{
    public interface IVehicleHandler
    {
        public Task<List<Vehicle>> GetVehiclesAsync();

        public Task<(bool, string)> RegisterVehicleAsync(Vehicle vehicle);
    }
}
