namespace VehicleTracking.API.Utility
{
    public class ErrorMessages
    {
        public const string VehicleAlreadyMapped =
            "This vehicle is already mapped to a device. Please disconnect from previous device and then register with new device.";

        public const string VehicleRegistrationException =
            "An error occurred while registering the vehicle.";

        public const string InvalidVin = "Invalid Vehicle Identification Number.";

        public const string LocationStoreException = "An error occurred while storing the location.";

        public const string LocationFetchForDurationException = "An error occurred while fetching location for the duration.";

        public const string CurrentLocationFetchException = "An error occurred while fetching the current location.";
    }
}
