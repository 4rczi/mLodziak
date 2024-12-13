using MlodziakApp.ApiCalls;
using MlodziakApp.ApiRequests;
using MlodziakApp.Services;
using Moq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mlodziak.UnitTests.ApiRequestsTests
{



    public class AccessTokenRequestsTests
    {
        private Mock<IApplicationLoggingRequests> _mockApplicationLogger;
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<ISecureStorageService> _mockSecureStorageService;
        private Mock<IAccessTokenApiCalls> _mockAccessTokenApiCalls;
        private AccessTokenRequests _accessTokenRequests;


        public AccessTokenRequestsTests()
        {
            _mockApplicationLogger = new Mock<IApplicationLoggingRequests>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockSecureStorageService = new Mock<ISecureStorageService>();
            _mockAccessTokenApiCalls = new Mock<IAccessTokenApiCalls>();

            _accessTokenRequests = new AccessTokenRequests(
                _mockApplicationLogger.Object,
                _mockHttpClientFactory.Object,
                _mockSecureStorageService.Object,
                _mockAccessTokenApiCalls.Object
            );
        }

        [Fact]
        public async Task ValidateAccessTokenAsync_ShouldReturnSuccess_WhenApiCallSucceeds()
        {
            // Arrange
            var accessToken = "validToken";
            var apiResponse = new ApiResponse<object>(new HttpResponseMessage(HttpStatusCode.OK), null, null);

            _mockAccessTokenApiCalls
                .Setup(x => x.GetAccessTokenAsync($"Bearer {accessToken}"))
                .ReturnsAsync(apiResponse);

            // Act
            var (isSuccess, statusCode) = await _accessTokenRequests.ValidateAccessTokenAsync(accessToken);

            // Assert
            Assert.True(isSuccess);
            Assert.Equal(HttpStatusCode.OK, statusCode);
            _mockAccessTokenApiCalls.Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ValidateAccessTokenAsync_ShouldReturnFalse_WhenApiCallFailes()
        {
            // Arrange
            var accessToken = "invalidToken";
            var apiResponse = new ApiResponse<object>(new HttpResponseMessage(HttpStatusCode.Unauthorized), null, null);

            _mockAccessTokenApiCalls
                .Setup(x => x.GetAccessTokenAsync($"Bearer {accessToken}"))
                .ReturnsAsync(apiResponse);

            // Act
            var (isSuccess, statusCode) = await (_accessTokenRequests.ValidateAccessTokenAsync(accessToken));

            // Assert
            Assert.False(isSuccess);
            Assert.Equal(HttpStatusCode.Unauthorized, statusCode);
            _mockAccessTokenApiCalls.Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ValidateAccessTokenAsync_ShouldLogErrorAndReturn500_WhenExceptionThrown()
        {
            // Arrange
            var accessToken = "exceptionToken";
            _mockAccessTokenApiCalls
                .Setup(x => x.GetAccessTokenAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("API error"));

            _mockSecureStorageService
                .Setup(x => x.GetUserIdAsync())
                .ReturnsAsync("testUser");
            _mockSecureStorageService
                .Setup(x => x.GetSessionIdAsync())
                .ReturnsAsync("testSession");

            // Act
            var (isSuccess, statusCode) = await _accessTokenRequests.ValidateAccessTokenAsync(accessToken);

            // Assert
            Assert.False(isSuccess);
            Assert.Equal(HttpStatusCode.InternalServerError, statusCode);

            _mockAccessTokenApiCalls.Verify(x => x.GetAccessTokenAsync(It.IsAny<string>()), Times.Once);
            _mockApplicationLogger.Verify(x => x.LogAsync(
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
