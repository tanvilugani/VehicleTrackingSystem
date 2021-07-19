using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking.API.Models;

namespace VehicleTracking.API.Utility
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TrackingRecord, LocationTracker>().ReverseMap();
        }
    }
}
