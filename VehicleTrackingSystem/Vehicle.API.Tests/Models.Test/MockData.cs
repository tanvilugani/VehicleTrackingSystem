using System;
using System.Collections.Generic;
using System.Text;
using VehicleTracking.API.Models;

namespace Vehicle.API.Tests.Models.Test
{
    class MockData
    {
        public const string RegistrationId = "60f444f548366da8fa8190f1";
        public static DateTime StartTime = new DateTime(2021, 07, 02, 10, 35, 0);
        public static DateTime EndTime = new DateTime(2021, 07, 02, 10, 50, 0);
        public static List<TrackingRecord> TrackingRecords = new List<TrackingRecord>()
        {
            new TrackingRecord()
            {
                RegistrationId = "60f444f548366da8fa8190f1",
                Location = new Location() { Latitude = 23.4555, Longitude = 34.1223 },
                Time = new DateTime(2021, 7, 3)
            },
            new TrackingRecord()
            {
                RegistrationId = "60f444f548366da8fa8190f1",
                Location = new Location() { Latitude = 65.4555, Longitude = 20.1223 },
                Time = new DateTime(2021, 7, 4)
            },

        };
    }
}
