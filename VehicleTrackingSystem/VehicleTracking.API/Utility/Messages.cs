namespace VehicleTracking.API.Utility
{
    public class Messages
    {
        public const string VehicleAlreadyMapped =
            "Vehicle is already mapped to a device.";

        public const string DeviceAlreadyMapped =
            "Device is already mapped to a vehicle.";

        public const string VehicleRegisteredSuccessfully =
            "Vehicle Registered Successfully.";
        
        public const string VehiclesFetchException =
            "An error occurred while fetching the vehicles.";

        public const string VehicleRegistrationException =
            "An error occurred while registering the vehicle.";

        public const string InvalidVin = "Invalid Vehicle Identification Number.";

        public const string LocationStoreException = "An error occurred while storing the location.";

        public const string LocationFetchForDurationException = "An error occurred while fetching location for the duration.";

        public const string CurrentLocationFetchException = "An error occurred while fetching the current location.";
    }
}
