using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking.API.Models;
using VehicleTracking.API.Repositories;

namespace Vehicle.API.Tests.Models.Test
{
    [TestFixture]
    class LocationTrackerTests
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LocationTracker> _logger;
        private ITrackerRepository _trackerRepository;
        private LocationTracker _locationTracker;

        public LocationTrackerTests()
        {
            _configuration = Substitute.For<IConfiguration>();
            _logger = Substitute.For<ILogger<LocationTracker>>();
        }

        [SetUp]
        public void Setup()
        {
            _trackerRepository = Substitute.For<ITrackerRepository>();
            _locationTracker = new LocationTracker(_configuration, _logger, _trackerRepository);
        }

        [Test]
        public async Task GetLocationsForDurationAsync_DataExists_ReturnsLocations()
        {
            // Arrange
            _trackerRepository.GetRecordsAsync(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(MockData.TrackingRecords);

            // Act
            var locations = await _locationTracker.GetLocationsForDurationAsync(MockData.RegistrationId, MockData.StartTime, MockData.EndTime);

            // Assert
            Assert.That(locations.Count > 0);
        }

        [Test]
        public async Task GetLocationsForDurationAsync_NoDataExists_ReturnsLocationsWithZeroCount()
        {
            // Arrange
            _trackerRepository.GetRecordsAsync(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).ReturnsNull();

            // Act
            var locations = await _locationTracker.GetLocationsForDurationAsync(MockData.RegistrationId, MockData.StartTime, MockData.EndTime);

            // Assert
            Assert.That(locations.Count == 0);
        }

        [Test]
        public void GetLocationsForDurationAsync_DatabaseThrowsException_ThrowsException()
        {
            // Arrange
            _trackerRepository.When(x => x.GetRecordsAsync(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()))
                .Do(x => { throw new Exception(); });

            // Act and Assert
            Assert.ThrowsAsync<Exception>(() =>
                   _locationTracker.GetLocationsForDurationAsync(MockData.RegistrationId, MockData.StartTime, MockData.EndTime)
               );
        }

        [TearDown]
        public void TearDown()
        {

        }
    }
}
