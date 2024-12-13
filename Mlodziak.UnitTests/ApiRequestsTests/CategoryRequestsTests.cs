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
using Xunit.Sdk;

namespace Mlodziak.UnitTests.ApiRequestsTests
{
    public class CategoryRequestsTests
    {
        private readonly Mock<IApplicationLoggingRequests> _mockApplicationLoggingRequests;
        private readonly Mock<ICategoryApiCalls> _mockCategoryApiCalls;
        private readonly ICategoryRequests _categoryRequests;

        public CategoryRequestsTests()
        {
            _mockApplicationLoggingRequests = new Mock<IApplicationLoggingRequests>();
            _mockCategoryApiCalls = new Mock<ICategoryApiCalls>();

            _categoryRequests = new CategoryRequests(_mockApplicationLoggingRequests.Object, _mockCategoryApiCalls.Object);
        }

        [Fact]
        public async Task GetCategoryModelsAsync_ShouldReturnCategoryModels_WhenApiCallSucceds()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var userId = "testUser";
            var sessionId = "testSession";
            var expectedResult = new List<CategoryModel>
            {
                new CategoryModel { Id = 1, Name = "Category1" },
                new CategoryModel { Id = 2, Name = "Category2" }
            };

            _mockCategoryApiCalls.Setup(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"))
                                 .ReturnsAsync(expectedResult);

            // Act
            var result = await _categoryRequests.GetCategoryModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult[0].Name, result[0].Name);
            _mockCategoryApiCalls.Verify(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"), Times.Once);
        }

        [Fact]
        public async Task GetCategoryModelsAsync_ShouldLogWarningAndReturnEmptyList_WhenApiCallThrowsApiException()
        {
            // Arrange
            var accessToken = "validAccessToken";
            var userId = "testUser";
            var sessionId = "testSession";
            var apiException = await ApiException.Create(
                null,
                null,
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = null
                },
                null);

            _mockCategoryApiCalls.Setup(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"))
                         .ThrowsAsync(apiException);

            // Act
            var result = await _categoryRequests.GetCategoryModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockCategoryApiCalls.Verify(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"), Times.Once);
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
        public async Task GetCategoryModelsAsync_ShouldLogErrorAndReturnEmptyList_WhenGeneralExceptionIsThrown()
        {
            var accessToken = "validAccessToken";
            var userId = "testUser";
            var sessionId = "testSession";
            var exception = new Exception("General exception");

            _mockCategoryApiCalls.Setup(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"))
                         .ThrowsAsync(exception);

            // Act
            var result = await _categoryRequests.GetCategoryModelsAsync(accessToken, userId, sessionId);

            // Assert
            Assert.Empty(result);
            _mockCategoryApiCalls.Verify(x => x.GetCategoriesAsync(userId, $"Bearer {accessToken}"), Times.Once);
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
