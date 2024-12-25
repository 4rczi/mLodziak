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
    public class LocationRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<ILocationApiCalls> _mockLocationApiCalls;
        private readonly ILocationRequests _locationRequests;

        public LocationRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockLocationApiCalls = new Mock<ILocationApiCalls>();
            _locationRequests = new LocationRequests(_mockApplicationLoggingRequests.Object, _mockLocationApiCalls.Object);
        }


        [Fact]
        public async Task GetLocationModelsAsync_ShouldReturnLocationModels_WhenApiCallSucceeds()
        {
            // Arrange
            var accessToken = "validToken";
            var categoryId = 1;
            var userId = "user123";
            var sessionId = "session123";
            var expectedResult = new List<LocationModel> { new LocationModel { Id = 1, Name = "Location 1" } };

            _mockLocationApiCalls.Setup(x => x.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId))
                                 .ReturnsAsync(expectedResult);

            // Act
            var result = await _locationRequests.GetLocationModelsAsync(accessToken, categoryId, userId, sessionId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetLocationModelsAsync_ShouldLogWarningAndReturnEmptyList_WhenApiCallThrowsApiException()
        {
            // Arrange
            var accessToken = "validToken";
            var categoryId = 1;
            var userId = "user123";
            var sessionId = "session999";
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockLocationApiCalls.Setup(x => x.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId))
                                 .ThrowsAsync(apiException);

            // Act
            var result = await _locationRequests.GetLocationModelsAsync(accessToken, categoryId, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockLocationApiCalls.Verify(x => x.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId), Times.Once);
            _mockApplicationLoggingRequests.Verify(x => x.LogAsync(
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
        public async Task GetLocationModelsAsync_ShouldLogErrorAndReturnEmptyList_WhenApiCallThrowsGeneralException()
        {
            // Arrange
            var accessToken = "validToken";
            var categoryId = 1;
            var userId = "user123";
            var sessionId = "session999";
            var apiException = new Exception("General exception");

            _mockLocationApiCalls.Setup(x => x.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId))
                                 .ThrowsAsync(apiException);

            // Act
            var result = await _locationRequests.GetLocationModelsAsync(accessToken, categoryId, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockLocationApiCalls.Verify(x => x.GetLocationModelsAsync($"Bearer {accessToken}", userId, categoryId), Times.Once);
            _mockApplicationLoggingRequests.Verify(x => x.LogAsync(
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

        [Fact]
        public async Task GetAllLocationModelsAsync_ShouldReturnLocationModels_WhenApiCallSucceeds()
        {
            // Arrange
            var accessToken = "validToken";
            var categoryId = 1;
            var userId = "user123";
            var sessionId = "session123";
            var expectedResult = new Dictionary<int, List<LocationModel>>
            {
                { 1, new List<LocationModel>
                    {
                        new LocationModel { Id = 1, Name = "Location 1"},
                        new LocationModel { Id = 2, Name = "Location 2"}
                    }
                }
            };

            _mockLocationApiCalls.Setup(x => x.GetAllLocationModelsAsync($"Bearer {accessToken}", userId))
                                 .ReturnsAsync(expectedResult);

            // Act
            var result = await _locationRequests.GetAllLocationModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task GetAllLocationModelsAsync_ShouldLogWarningAndReturnEmptyList_WhenApiCallThrowsApiException()
        {
            // Arrange
            var accessToken = "validToken";
            var userId = "user123";
            var sessionId = "session999";
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockLocationApiCalls.Setup(x => x.GetAllLocationModelsAsync($"Bearer {accessToken}", userId))
                                 .ThrowsAsync(apiException);

            // Act
            var result = await _locationRequests.GetAllLocationModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockLocationApiCalls.Verify(x => x.GetAllLocationModelsAsync($"Bearer {accessToken}", userId), Times.Once);
            _mockApplicationLoggingRequests.Verify(x => x.LogAsync(
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
        public async Task GetAllLocationModelsAsync_ShouldLogErrorAndReturnEmptyList_WhenApiCallThrowsGeneralException()
        {
            // Arrange
            var accessToken = "validToken";
            var categoryId = 1;
            var userId = "user123";
            var sessionId = "session999";
            var apiException = new Exception("General exception");

            _mockLocationApiCalls.Setup(x => x.GetAllLocationModelsAsync($"Bearer {accessToken}", userId))
                                 .ThrowsAsync(apiException);

            // Act
            var result = await _locationRequests.GetAllLocationModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockLocationApiCalls.Verify(x => x.GetAllLocationModelsAsync($"Bearer {accessToken}", userId), Times.Once);
            _mockApplicationLoggingRequests.Verify(x => x.LogAsync(
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
