using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Repositories;
using VehicleTracking.API.Utility;

namespace VehicleTracking.API.Handlers
{
    public class VehicleHandler : IVehicleHandler
    {
        private readonly ILogger<VehicleHandler> _logger;
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleHandler(ILogger<VehicleHandler> logger, IVehicleRepository vehicleRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }

        public async Task<List<Vehicle>> GetVehiclesAsync()
        {
            try
            {
                return await _vehicleRepository.GetVehiclesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching vehicles.");
                throw ex;
            }
        }

        public async Task<(bool, string)> RegisterVehicleAsync(Vehicle vehicle)
        {
            try
            {
                var isVehicleMapped = await IsVehicleMapped(vehicle.VehicleIdentificationNumber);

                if (isVehicleMapped)
                {
                    return (false, Messages.VehicleAlreadyMapped);
                }

                var isDeviceMapped = await IsDeviceMapped(vehicle.MappedDeviceId);
                
                if (isDeviceMapped)
                {
                    return (false, Messages.DeviceAlreadyMapped);
                }

                await _vehicleRepository.RegisterVehicleAsync(vehicle);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Exception while adding vehicle with VIN : {vehicle.VehicleIdentificationNumber}", ex.Message);
                throw ex;
            }

            return (true, Messages.VehicleRegisteredSuccessfully);
        }

        private async Task<bool> IsVehicleMapped(string vehicleIdentificationNumber)
        {
            return await _vehicleRepository.IsVehicleMapped(vehicleIdentificationNumber);
        }

        private async Task<bool> IsDeviceMapped(string deviceId)
        {
            return await _vehicleRepository.IsDeviceMapped(deviceId);

        }
    }
}
