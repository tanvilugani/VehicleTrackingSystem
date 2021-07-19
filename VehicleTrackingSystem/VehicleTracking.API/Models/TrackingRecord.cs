using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace VehicleTracking.API.Models
{
    public class TrackingRecord
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Required]
        public string RegistrationId { get; set; }
        
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Time { get; set; }

        public Location Location { get; set; }
    }
}
