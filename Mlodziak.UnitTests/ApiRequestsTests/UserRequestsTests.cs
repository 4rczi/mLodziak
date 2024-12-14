using DataAccess.Entities;
using MlodziakApp.ApiCalls;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using Moq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mlodziak.UnitTests.ApiRequestsTests
{
    public class UserRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<IUserApiCalls> _mockUserApiCalls;
        private readonly Mock<ISecureStorageService> _mockSecureStorageService;
        private readonly UserRequests _userRequests;

        public UserRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockUserApiCalls = new Mock<IUserApiCalls>();
            _mockSecureStorageService = new Mock<ISecureStorageService>();
            _userRequests = new UserRequests(
                _mockApplicationLoggingRequests.Object,
                _mockUserApiCalls.Object,
                _mockSecureStorageService.Object
            );
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnUser_WhenApiCallSucceeds()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";
            var expectedUser = new User { Id = userId };

            _mockUserApiCalls
                .Setup(api => api.GetUserAsync(userId, $"Bearer {accessToken}"))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _userRequests.GetUserAsync(userId, accessToken);

            // Assert
            Assert.Equal(expectedUser, result);
            _mockUserApiCalls.Verify(api => api.GetUserAsync(userId, $"Bearer {accessToken}"), Times.Once);
        }

        [Fact]
        public async Task GetUserAsync_ShouldReturnNull_WhenApiExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockUserApiCalls
                .Setup(api => api.GetUserAsync(userId, $"Bearer {accessToken}"))
                .ThrowsAsync(apiException);

            // Act
            var result = await _userRequests.GetUserAsync(userId, accessToken);

            // Assert
            Assert.Null(result);
            _mockUserApiCalls.Verify(api => api.GetUserAsync(userId, $"Bearer {accessToken}"), Times.Once);
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
        public async Task GetUserAsync_ShouldReturnNull_WhenGeneralExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";

            _mockUserApiCalls
                .Setup(api => api.GetUserAsync(userId, $"Bearer {accessToken}"))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _userRequests.GetUserAsync(userId, accessToken);

            // Assert
            Assert.Null(result);
            _mockUserApiCalls.Verify(api => api.GetUserAsync(userId, $"Bearer {accessToken}"), Times.Once);
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
        public async Task CreateNewUserAsync_ShouldReturnTrue_WhenApiCallSucceeds()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";
            var user = new User { Id = userId };

            _mockUserApiCalls
                .Setup(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"))
                .ReturnsAsync(true);

            // Act
            var result = await _userRequests.CreateNewUserAsync(userId, accessToken);

            // Assert
            Assert.True(result);
            _mockUserApiCalls.Verify(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"), Times.Once);
        }

        [Fact]
        public async Task CreateNewUserAsync_ShouldReturnFalse_WhenApiExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";
            var user = new User { Id = userId };
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockUserApiCalls
                .Setup(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"))
                .ThrowsAsync(apiException);

            // Act
            var result = await _userRequests.CreateNewUserAsync(userId, accessToken);

            // Assert
            Assert.False(result);
            _mockUserApiCalls.Verify(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"), Times.Once);
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
        public async Task CreateNewUserAsync_ShouldReturnFalse_WhenGeneralExceptionIsThrown()
        {
            // Arrange
            var userId = "TestUserId";
            var accessToken = "validAccessToken";
            var user = new User { Id = userId };

            _mockUserApiCalls
                .Setup(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _userRequests.CreateNewUserAsync(userId, accessToken);

            // Assert
            Assert.False(result);
            _mockUserApiCalls.Verify(api => api.CreateNewUserAsync(It.Is<User>(u => u.Id == userId), $"Bearer {accessToken}"), Times.Once);
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
