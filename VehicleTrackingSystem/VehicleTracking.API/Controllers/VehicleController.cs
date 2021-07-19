using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Repositories;
using VehicleTracking.API.Utility;

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
            try
            {
                var existingVehicle = await _vehicleRepository.GetVehicleUsingVehicleIdNumber(vehicle.VehicleIdentificationNumber);

                if (existingVehicle != null && existingVehicle.IsActive)
                {
                    return BadRequest(ErrorMessages.VehicleAlreadyMapped);
                }

                await _vehicleRepository.RegisterAsync(vehicle);

                return Ok(vehicle.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Exception while adding vehicle with VIN : {vehicle.VehicleIdentificationNumber}. Exception Message : {ex.Message}");
                return BadRequest(ErrorMessages.VehicleRegistrationException);
            }
            
        }
    }
}
