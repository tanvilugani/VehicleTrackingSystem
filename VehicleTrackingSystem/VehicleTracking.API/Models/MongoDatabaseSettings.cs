namespace VehicleTracking.API.Models
{
    public class MongoDatabaseSettings : IDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set ; }
    }
}
