using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Repositories;

namespace VehicleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(ILogger<VehicleController> logger, IVehicleRepository vehicleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> RegisterAsync(Vehicle vehicle)
        {
            var existingVehicle = await _vehicleRepository.GetVehicleUsingVehicleIdNumber(vehicle.VehicleIdentificationNumber);

            if(existingVehicle != null && existingVehicle.IsActive)
            {
                return BadRequest("This vehicle is already mapped to a device.");
            }

            await _vehicleRepository.RegisterAsync(vehicle);

            return Ok(vehicle.Id);
        }
    }
}
