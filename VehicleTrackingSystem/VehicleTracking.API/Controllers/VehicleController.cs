using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Handlers;
using VehicleTracking.API.Models;
using VehicleTracking.API.Utility;

namespace VehicleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ILogger<VehicleController> _logger;
        private readonly IVehicleHandler _vehicleHandler;

        public VehicleController(ILogger<VehicleController> logger, IVehicleHandler vehicleHandler)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleHandler = vehicleHandler ?? throw new ArgumentNullException(nameof(vehicleHandler));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Vehicle>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Vehicle>>> GetAsync()
        {
            try
            {
                var vehicles = await _vehicleHandler.GetVehiclesAsync();

                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Exception while fetching vehicles.", ex.Message);

                return BadRequest(Messages.VehiclesFetchException);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<int>> RegisterAsync(Vehicle vehicle)
        {
            try
            {
                var result = await _vehicleHandler.RegisterVehicleAsync(vehicle);

                if (!result.Item1)
                {
                    return BadRequest(result.Item2);
                }
                    
                return Ok(vehicle.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Exception while adding vehicle with VIN : {vehicle.VehicleIdentificationNumber}.", ex.Message);
               
                return BadRequest(Messages.VehicleRegistrationException);
            }
        }
    }
}
