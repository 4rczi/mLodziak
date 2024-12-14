using MlodziakApp.ApiCalls;
using MlodziakApp.ApiRequests;
using Moq;
using Refit;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlodziak.UnitTests.ApiRequestsTests
{
    public class PhysicalLocationRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<IPhysicalLocationApiCalls> _mockPhysicalLocationApiCalls;
        private readonly PhysicalLocationRequests _physicalLocationRequests;

        public PhysicalLocationRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockPhysicalLocationApiCalls = new Mock<IPhysicalLocationApiCalls>();

            _physicalLocationRequests = new PhysicalLocationRequests(
                _mockApplicationLoggingRequests.Object,
                _mockPhysicalLocationApiCalls.Object
            );
        }

        [Fact]
        public async Task GetPhysicalLocationModelsAsync_ShouldReturnLocations_WhenApiCallSucceeds()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var userId = "TestUserId";
            var sessionId = "TestSessionId";
            var categoryId = 1;
            var locationId = 2;
            var physicalLocations = new List<PhysicalLocationModel>
        {
            new PhysicalLocationModel { Id = 1, Name = "Location1" },
            new PhysicalLocationModel { Id = 2, Name = "Location2" }
        };

            _mockPhysicalLocationApiCalls
                .Setup(api => api.GetPhysicalLocationsAsync(userId, categoryId, locationId, $"Bearer {accessToken}"))
                .ReturnsAsync(physicalLocations);

            // Act
            var result = await _physicalLocationRequests.GetPhysicalLocationModelsAsync(
                accessToken, userId, categoryId, locationId, sessionId);

            // Assert
            Assert.Equal(physicalLocations, result);
        }

        [Fact]
        public async Task GetPhysicalLocationModelsAsync_ShouldReturnEmptyList_WhenApiExceptionIsThrown()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var userId = "TestUserId";
            var sessionId = "TestSessionId";
            var categoryId = 1;
            var locationId = 2;
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockPhysicalLocationApiCalls
                .Setup(api => api.GetPhysicalLocationsAsync(userId, categoryId, locationId, $"Bearer {accessToken}"))
                .ThrowsAsync(apiException);

            // Act
            var result = await _physicalLocationRequests.GetPhysicalLocationModelsAsync(
                accessToken, userId, categoryId, locationId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockApplicationLoggingRequests.Verify(log => log.LogAsync(
                "Warning",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()),
                Times.Once);
        }

        [Fact]
        public async Task GetPhysicalLocationModelsAsync_ShouldReturnEmptyList_WhenGeneralExceptionIsThrown()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var userId = "TestUserId";
            var sessionId = "TestSessionId";
            var categoryId = 1;
            var locationId = 2;
            var generalException = new Exception("General error");

            _mockPhysicalLocationApiCalls
                .Setup(api => api.GetPhysicalLocationsAsync(userId, categoryId, locationId, $"Bearer {accessToken}"))
                .ThrowsAsync(generalException);

            // Act
            var result = await _physicalLocationRequests.GetPhysicalLocationModelsAsync(
                accessToken, userId, categoryId, locationId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockApplicationLoggingRequests.Verify(log => log.LogAsync(
                "Error",
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()),
                Times.Once);
        }
    }
}
