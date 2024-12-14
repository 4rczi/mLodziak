using DataAccess.Entities;
using MlodziakApp.ApiCalls;
using MlodziakApp.ApiRequests;
using Moq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlodziak.UnitTests.ApiRequestsTests
{
    public class UserHistoryRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<IUserHistoryApiCalls> _mockUserHistoryApiCalls;
        private readonly UserHistoryRequests _userHistoryRequests;

        public UserHistoryRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockUserHistoryApiCalls = new Mock<IUserHistoryApiCalls>();

            _userHistoryRequests = new UserHistoryRequests(
                _mockApplicationLoggingRequests.Object,
                _mockUserHistoryApiCalls.Object
            );
        }

        [Fact]
        public async Task CreateUserHistoryAsync_ShouldReturnTrue_WhenApiCallSucceeds()
        {
            // Arrange
            var userId = "TestUserId";
            var physicalLocationId = 1;
            var accessToken = "validAccessToken";
            var sessionId = "TestSessionId";
            var userHistory = new UserHistory
            {
                UserId = userId,
                PhysicalLocationId = physicalLocationId
            };

            _mockUserHistoryApiCalls
                .Setup(api => api.CreateUserHistoryAsync(It.Is<UserHistory>(uh => uh.UserId == userId && uh.PhysicalLocationId == physicalLocationId), $"Bearer {accessToken}"))
                .ReturnsAsync(true);

            // Act
            var result = await _userHistoryRequests.CreateUserHistoryAsync(
                userId, physicalLocationId, accessToken, sessionId);

            // Assert
            Assert.True(result);
            _mockUserHistoryApiCalls.Verify(x => x.CreateUserHistoryAsync(It.Is<UserHistory>(uh => uh.UserId == userId && uh.PhysicalLocationId == physicalLocationId), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task CreateUserHistoryAsync_ShouldReturnFalse_WhenApiExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var physicalLocationId = 1;
            var accessToken = "validAccessToken";
            var sessionId = "TestSessionId";
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockUserHistoryApiCalls
                .Setup(api => api.CreateUserHistoryAsync(It.IsAny<UserHistory>(), $"Bearer {accessToken}"))
                .ThrowsAsync(apiException);

            // Act
            var result = await _userHistoryRequests.CreateUserHistoryAsync(userId, physicalLocationId, accessToken, sessionId);

            // Assert
            Assert.False(result);
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
        public async Task CreateUserHistoryAsync_ShouldReturnFalse_WhenGeneralExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var physicalLocationId = 1;
            var accessToken = "validAccessToken";
            var sessionId = "TestSessionId";
            var generalException = new Exception("General error");

            _mockUserHistoryApiCalls
                .Setup(api => api.CreateUserHistoryAsync(It.IsAny<UserHistory>(), $"Bearer {accessToken}"))
                .ThrowsAsync(generalException);

            // Act
            var result = await _userHistoryRequests.CreateUserHistoryAsync(userId, physicalLocationId, accessToken, sessionId);

            // Assert
            Assert.False(result);
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
