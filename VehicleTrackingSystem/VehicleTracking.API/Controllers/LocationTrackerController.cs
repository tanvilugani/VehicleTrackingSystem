using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Utility;

namespace VehicleTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationTrackerController : ControllerBase
    {
        private readonly ILogger<LocationTrackerController> _logger;
        private readonly ILocationTracker _locationTracker;

        public LocationTrackerController(ILogger<LocationTrackerController> logger, ILocationTracker location)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _locationTracker = location ?? throw new ArgumentNullException(nameof(location));
        }

        [HttpGet("{registrationId}")]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<string>> GetAsync(string registrationId)
        {
            try
            {
                var locality = await _locationTracker.GetCurrentLocationAsync(registrationId);

                if (locality == null)
                {
                    return NoContent();
                }

                return Ok(locality);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error while fetching location for the registration ID {registrationId}.", ex.Message);
                return BadRequest(ErrorMessages.CurrentLocationFetchException); ;
            }
        }

        [HttpGet("locations/{registrationId}")]
        [ProducesResponseType(typeof(IList<Location>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult<IList<Location>>> GetLocationForDurationAsync(string registrationId, DateTime startTime, DateTime endTime)
        {
            try
            {
                var locations = await _locationTracker.GetLocationsForDurationAsync(registrationId, startTime, endTime);

                if (locations == null || locations.Count == 0)
                {
                    return NoContent();
                }

                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error while fetching locations for the registration ID {registrationId}.", ex.Message);
                return BadRequest(ErrorMessages.LocationFetchForDurationException);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TrackingRecord>> PostAsync(TrackingRecord record)
        {
            try
            {
                await _locationTracker.AddLocationAsync(record);

                return Ok(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error while storing location for the registration ID {record.RegistrationId}.", ex.Message);
                return BadRequest(ErrorMessages.LocationStoreException);
            }
        }
    }
}
