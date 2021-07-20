using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VehicleTracking.API.Models
{
    public class GeoCodingResponse
    {
        [JsonPropertyName("plus_code")]
        public Codes PlusCode { get; set; }
        
        [JsonPropertyName("results")]
        public List<ResultObjects> Results { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
       
    }

    public class Codes
    {
        [JsonPropertyName("compound_code")]
        public string CompoundCode { get; set; }

        [JsonPropertyName("global_code")]
        public string GlobalCode { get; set; }
    }

    public class ResultObjects
    {
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; }

        [JsonPropertyName("address_components")]
        public List<AddressComponents> AddressComponents { get; set; }

        [JsonPropertyName("geometry")]
        [JsonIgnore]
        public Geometry Geometry { get; set; }

        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
    }

    public class AddressComponents
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; }
        
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; }

        [JsonPropertyName("types")]
        public List<string> Types { get; set; }
    }

    public class Geometry
    {
        
    }
}
