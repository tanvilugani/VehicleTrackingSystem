using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using VehicleTracking.API.Utility;

namespace VehicleTracking.API.Models
{
    public class Vehicle
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        [RegularExpression("^([A-Z0-9]){17}$", ErrorMessage = Messages.InvalidVin)]
        public string VehicleIdentificationNumber { get; set; }
        [Required]
        public string VehicleNumber { get; set; }
        public string Model { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RegisteredOn { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string MappedDeviceId { get; set; }
    }
}
