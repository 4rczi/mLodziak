using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using MlodziakApp;
using MlodziakApp.Services;
using Moq;
using Xunit;

namespace Mlodziak.UnitTests
{
    
    public class SecureStorageServiceTests
    {
        private readonly Mock<ISecureStorageWrapper> _secureStorageWrapper;
        private readonly SecureStorageService _secureStorageService;


        public SecureStorageServiceTests()
        {
            _secureStorageWrapper = new Mock<ISecureStorageWrapper>();
            _secureStorageService = new SecureStorageService(_secureStorageWrapper.Object);
        }

        [Fact]
        public async Task GetAccessTokenAsync_CallsGetAsyncWithCorrectKey()
        {
            // Act
            await _secureStorageService.GetAccessTokenAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.GetAsync("accessToken"), Times.Once);
        }

        [Fact]
        public async Task SetAccessTokenAsync_CallsSetAsyncWithCorrectKeyAndValue()
        {
            // Act
            await _secureStorageService.SetAccessTokenAsync("testToken");

            // Assert
            _secureStorageWrapper.Verify(x => x.SetAsync("accessToken", "testToken"), Times.Once);
        }

        [Fact]
        public async Task RemoveAccessTokenAsync_CallsRemoveAsyncWithCorrectKey()
        {
            // Act
            await _secureStorageService.RemoveAccessTokenAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.RemoveAsync("accessToken"), Times.Once);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_CallsGetAsyncWithCorrectKey()
        {
            // Act
            await _secureStorageService.GetRefreshTokenAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.GetAsync("refreshToken"), Times.Once);
        }

        [Fact]
        public async Task SetRefreshTokenAsync_CallsSetAsyncWithCorrectKeyAndValue()
        {
            // Act
            await _secureStorageService.SetRefreshTokenAsync("testToken");

            // Assert
            _secureStorageWrapper.Verify(x => x.SetAsync("refreshToken", "testToken"), Times.Once);
        }

        [Fact]
        public async Task RemoveRefreshTokenAsync_CallsRemoveAsyncWithCorrectKey()
        {
            // Act
            await _secureStorageService.RemoveRefreshTokenAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.RemoveAsync("refreshToken"), Times.Once);
        }

        [Fact]
        public async Task GetSessionIdAsync_CallsGetAsyncWithCorrectKey()
        {
            // Act
            await _secureStorageService.GetSessionIdAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.GetAsync("sessionId"), Times.Once);
        }

        [Fact]
        public async Task SetSessionIdAsync_CallsSetAsyncWithCorrectKeyAndValue()
        {
            // Act
            await _secureStorageService.SetSessionIdAsync("sessionId");

            // Assert
            _secureStorageWrapper.Verify(x => x.SetAsync("sessionId", "sessionId"),Times.Once);
        }

        [Fact]
        public async Task RemoveSessionIdAsync_CallsRemoveWithCorrectKey()
        {
            // Act
            await _secureStorageService.RemoveSessionIdAsync();

            // Assert
            _secureStorageWrapper.Verify(x => x.RemoveAsync("sessionId"),Times.Once);
        }

        [Fact]
        public async Task GetAccessTokenAsync_ShouldReturnCorrectValueWithCorrectKey()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                 .ReturnsAsync("testToken");

            // Act
            var result = await _secureStorageService.GetAccessTokenAsync();

            // Assert
            Assert.Equal("testToken", result);
        }

        [Fact]
        public async Task GetAccessTokenAsync_ShouldReturnNullValue_WhenKeyDoesNotExist()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("nonExistingKey"))
                                 .ReturnsAsync(value:null);

            // Act
            var result = await _secureStorageService.GetAccessTokenAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SetAccessTokenAsync_ShouldSetKey_WhenKeyDoesNotExist()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                 .ReturnsAsync(value: null);

            // Act
            await _secureStorageService.SetAccessTokenAsync("accessToken");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                 .ReturnsAsync("testToken");


            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("accessToken");
            Assert.Equal("testToken", result);
        }



        [Fact]
        public async Task RemoveAccessTokenAsync_ShouldMakeKeyUnretrievable()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                        .ReturnsAsync("testToken");

            // Act
            await _secureStorageService.RemoveAccessTokenAsync();

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                        .ReturnsAsync(value:null);

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("accessToken");     
            Assert.Null(result);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_ShouldReturnNullValue_WhenKeyDoesNotExist()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("nonExistingKey"))
                                 .ReturnsAsync(value: null);

            // Act
            var result = await _secureStorageService.GetRefreshTokenAsync();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SetRefreshTokenAsync_ShouldSetKey_WhenKeyDoesNotExist()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync(value: null);

            // Act
            await _secureStorageService.SetRefreshTokenAsync("refreshToken");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync("testToken");

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("refreshToken");
            Assert.Equal("testToken", result);
        }

        [Fact]
        public async Task RemoveRefreshTokenAsync_ShouldMakeKeyUnretrievable()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync("testToken");

            // Act
            await _secureStorageService.RemoveRefreshTokenAsync();

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync(value: null);

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("refreshToken");
            Assert.Null(result);
        }

        [Fact]
        public async Task SetAccessTokenAsync_ShouldReplaceOldValue()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                 .ReturnsAsync("oldToken");

            // Act
            await _secureStorageService.SetAccessTokenAsync("newToken");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("accessToken"))
                                 .ReturnsAsync("newToken");

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("accessToken");
            Assert.Equal("newToken", result);
        }

        [Fact]
        public async Task SetRefreshTokenAsync_ShouldReplaceOldValue()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync("oldToken");

            // Act
            await _secureStorageService.SetAccessTokenAsync("newToken");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("refreshToken"))
                                 .ReturnsAsync("newToken");

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("refreshToken");
            Assert.Equal("newToken", result);
        }

        [Fact]
        public async Task SetSessionIdAsync_ShouldReplaceOldValue()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("sessionId"))
                                 .ReturnsAsync("oldSessionId");

            // Act
            await _secureStorageService.SetAccessTokenAsync("newSessionId");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("sessionId"))
                                 .ReturnsAsync("newSessionId");

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("sessionId");
            Assert.Equal("newSessionId", result);
        }

        [Fact]
        public async Task GetUserIdAsync_ShouldReturnCorrectValueWithCorrectKey()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("userId"))
                                 .ReturnsAsync("testUser");

            // Act
            var result = await _secureStorageService.GetUserIdAsync();

            // Assert
            Assert.Equal("testUser", result);
        }

        [Fact]
        public async Task SetUserIdAsync_ShouldReplaceOldValue()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("userId"))
                                 .ReturnsAsync("oldUser");

            // Act
            await _secureStorageService.SetUserIdAsync("newUser");

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("userId"))
                                 .ReturnsAsync("newUser");

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("userId");
            Assert.Equal("newUser", result);
        }

        [Fact]
        public async Task RemoveUserIdAsync_ShouldMakeKeyUnretrievable()
        {
            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("userId"))
                                 .ReturnsAsync("testUser");

            // Act
            await _secureStorageService.RemoveUserIdAsync();

            // Arrange
            _secureStorageWrapper.Setup(x => x.GetAsync("userId"))
                                 .ReturnsAsync(value: null);

            // Assert
            var result = await _secureStorageWrapper.Object.GetAsync("userId");
            Assert.Null(result);
        }
    }
}
