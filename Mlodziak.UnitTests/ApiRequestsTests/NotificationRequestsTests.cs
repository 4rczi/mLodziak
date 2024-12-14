using MlodziakApp.ApiCalls;
using MlodziakApp.ApiRequests;
using MlodziakApp.Logic.Notification;
using MlodziakApp.Services;
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
    public class NotificationRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<INotificationApiCalls> _mockNotificationApiCalls;
        private readonly Mock<ISecureStorageService> _mockSecureStorageService;
        private readonly NotificationRequests _notificationRequests;

        public NotificationRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockNotificationApiCalls = new Mock<INotificationApiCalls>();
            _mockSecureStorageService = new Mock<ISecureStorageService>();

            _notificationRequests = new NotificationRequests(
                _mockApplicationLoggingRequests.Object,
                _mockNotificationApiCalls.Object,
                _mockSecureStorageService.Object);
        }

        [Fact]
        public async Task SendFCMNotificationMessageRequestAsync_ShouldReturnTrue_WhenApiCallSucceeds()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var notificationRequest = new NotificationRequestModel();


            _mockNotificationApiCalls
                .Setup(api => api.SendNotificationAsync(notificationRequest, $"Bearer {accessToken}"))
                .ReturnsAsync("");

            // Act
            var result = await _notificationRequests.SendFCMNotificationMessageRequestAsync(accessToken, notificationRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SendFCMNotificationMessageRequestAsync_ShouldReturnFalse_WhenApiExceptionIsThrown()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var notificationRequest = new NotificationRequestModel();

            // Simulate an ApiException
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Content = null
                },
                null);

            _mockNotificationApiCalls
                .Setup(api => api.SendNotificationAsync(notificationRequest, $"Bearer {accessToken}"))
                .ThrowsAsync(apiException);

            // Act
            var result = await _notificationRequests.SendFCMNotificationMessageRequestAsync(accessToken, notificationRequest);

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
        public async Task SendFCMNotificationMessageRequestAsync_ShouldReturnFalse_WhenGeneralExceptionIsThrown()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var notificationRequest = new NotificationRequestModel();

            var generalException = new Exception("General error");
            _mockNotificationApiCalls
                .Setup(api => api.SendNotificationAsync(notificationRequest, $"Bearer {accessToken}"))
                .ThrowsAsync(generalException);

            // Act
            var result = await _notificationRequests.SendFCMNotificationMessageRequestAsync(accessToken, notificationRequest);

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
