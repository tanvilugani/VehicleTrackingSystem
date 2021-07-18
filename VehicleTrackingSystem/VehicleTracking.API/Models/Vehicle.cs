using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleTracking.API.Models
{
    public class Vehicle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [RegularExpression("^([A-Z0-9]){17}$", ErrorMessage = "Enter a valid VIN number")]
        public string VehicleIdentificationNumber { get; set; }
        [Required]
        public string VehicleNumber { get; set; }
        public string Model { get; set; }
        public DateTime YearOfManufacturing { get; set; }
        public DateTime RegisteredOn { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string MappedDeviceId { get; set; }
    }
}
